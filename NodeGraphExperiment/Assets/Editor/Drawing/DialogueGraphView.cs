using System;
using System.Collections.Generic;
using System.Linq;
using Editor.AssetManagement;
using Editor.ContextualMenu;
using Editor.Drawing.Nodes;
using Editor.Exporters;
using Editor.Factories;
using Editor.Factories.NodeListeners;
using Editor.Importers;
using Editor.Serialization;
using Editor.Undo;
using Editor.Undo.Commands;
using Runtime;
using Runtime.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Drawing
{
    public class DialogueGraphView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<DialogueGraphView, UxmlTraits> { }

        private CopyPasteNodes _copyPaste;
        private INodeViewFactory _factory;
        private RedirectNodeFactory _redirectFactory;
        private ContextualMenuBuilder _contextualMenu;
        private DialogueGraphContainer _graphContainer;
        private IUndoRegister _undoRegister;
        private NodesProvider _nodesProvider;

        public DialogueGraphView()
        {
            EditorPrefs.SetBool("GraphSnapping", false);
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer() { maxScale = 2f, minScale = 0.1f});
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            var stylesheet = Resources.Load<StyleSheet>("Styles/DialogueGraph");
            styleSheets.Add(stylesheet);
            RegisterCallback<DragUpdatedEvent>(OnDragUpdated);
            RegisterCallback<DragPerformEvent>(OnDragPerformEvent);
            RegisterCallback<MouseDownEvent>(OnMouseDown, TrickleDown.TrickleDown);

            graphViewChanged = OnGraphViewChanged;
            serializeGraphElements += OnCutCopyOperation;
            unserializeAndPaste += OnPasteOperation;
        }

        public event Action Saved;

        private void OnDragPerformEvent(DragPerformEvent evt)
        {
            var selection = DragAndDrop.GetGenericData("DragSelection") as List<ISelectable>;
            IEnumerable<BlackboardField> fields = selection.OfType<BlackboardField>();
            foreach (var field in fields) 
                _factory.CreateVariable(new VariableNode()
                {
                    Guid = Guid.NewGuid().ToString(),
                    Name = field.text,
                    Position = contentViewContainer.WorldToLocal(new Rect(evt.mousePosition, Vector2.zero))
                });
        }

        private void OnDragUpdated(DragUpdatedEvent e)
        {
            if (DragAndDrop.GetGenericData("DragSelection") is List<ISelectable> selection && (selection.OfType<BlackboardField>().Count() >= 0))
                DragAndDrop.visualMode = e.actionKey ? DragAndDropVisualMode.Copy : DragAndDropVisualMode.Move;
        }

        private string OnCutCopyOperation(IEnumerable<GraphElement> elements)
        {
            _copyPaste.Clear();
            foreach (var element in elements) 
                _copyPaste.Add(element);
            
            return "not empty string";
        }

        private void OnPasteOperation(string operationName, string data)
        {
            ClearSelection();
            _copyPaste.Paste(this);
        }

        public void Initialize(NodesProvider nodesProvider, UndoNodeViewFactory factory, RedirectNodeFactory redirectNodeFactory, 
            CopyPasteNodes copyPasteNodes, ContextualMenuBuilder contextualMenuBuilder, IUndoRegister undoRegister)
        {
            _nodesProvider = nodesProvider;
            _factory = factory;
            _contextualMenu = contextualMenuBuilder;
            _undoRegister = undoRegister;
            _copyPaste = copyPasteNodes;
            _redirectFactory = redirectNodeFactory;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.movedElements != null)
            {
                var movableNodes = graphViewChange.movedElements.OfType<IMovableNode>().ToArray();
                var moveCommand = new MoveNodes(movableNodes);
                _undoRegister.Register(moveCommand);
                foreach (var element in movableNodes) 
                    element.SavePosition(element.GetPosition());
            }

            if (graphViewChange.edgesToCreate != null)
            {
                _undoRegister.Register(new CreateEdges(this, graphViewChange.edgesToCreate));
            }

            if (graphViewChange.elementsToRemove != null)
            {
                _undoRegister.Register(new RemoveElements(this, graphViewChange.elementsToRemove));
            }
            
            return graphViewChange;
        }

        private void OnMouseDown(MouseDownEvent evt)
        {
            if (evt.clickCount >= 2 && evt is { target: Edge edge }) 
                _redirectFactory.CreateRedirect(edge, evt.mousePosition, OnMouseDown);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) =>
            ports.ToList();

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt) =>
            _contextualMenu.BuildContextualMenu(evt, base.BuildContextualMenu);

        public void Find(DialogueNodeView view)
        {
            var viewPosition = view.GetPosition();
            MoveTo(viewPosition);
            
            ClearSelection();
            AddToSelection(view);
        }

        public void MoveTo(Rect target)
        {
            CalculateFrameTransform(target, layout, 0, out var translation, out var scaling);
            UpdateViewTransform(translation, scaling);
        }

        public void Populate(DialogueGraphContainer graph)
        {
            _graphContainer = graph;
            var importer = new DialogueGraphImporter(this, _factory, _nodesProvider);
            importer.Import(graph.Graph);
        }

        public void Save()
        {
            var exporter = new DialogueGraphExporter(this, _nodesProvider, _graphContainer);
            exporter.Export();
            Saved?.Invoke();
        }
    }
}