using System;
using System.Collections.Generic;
using System.Linq;
using Editor.Drawing.Nodes;
using Editor.Exporters;
using Editor.Factories;
using Editor.Factories.NodeListeners;
using Editor.Importers;
using Editor.Serialization;
using Editor.Undo;
using Editor.Undo.Commands;
using Editor.Windows.Variables;
using Runtime;
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
        private DialogueGraphContainer _graphContainer;
        private IUndoRegister _undoRegister;
        private NodesProvider _nodesProvider;
        private VariablesProvider _variablesProvider;

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
            RegisterCallback<MouseDownEvent>(OnMouseDown, TrickleDown.TrickleDown);

            graphViewChanged = OnGraphViewChanged;
            serializeGraphElements += OnCutCopyOperation;
            unserializeAndPaste += OnPasteOperation;
        }

        public event Action Saved;

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
            CopyPasteNodes copyPasteNodes, IUndoRegister undoRegister, VariablesProvider variablesProvider)
        {
            _nodesProvider = nodesProvider;
            _factory = factory;
            _undoRegister = undoRegister;
            _copyPaste = copyPasteNodes;
            _redirectFactory = redirectNodeFactory;
            _variablesProvider = variablesProvider;
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
            var importer = new DialogueGraphImporter(this, _factory, _nodesProvider, _variablesProvider);
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