using System.Collections.Generic;
using System.Linq;
using Editor.Drawing.Nodes;
using Editor.Factories;
using Editor.Serialization;
using Runtime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class DialogueGraphView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<DialogueGraphView, UxmlTraits> { }

        private DialogueNodeFactory _factory;
        private ContextualMenuBuilder _contextualMenu;
        private readonly CopyPasteNodes _copyPaste;
        private VariableNodeFactory _variableFactory;

        public DialogueGraphView()
        {
            _copyPaste = new CopyPasteNodes();
            
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer() { maxScale = 2f, minScale = 0.1f});
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            var stylesheet = Resources.Load<StyleSheet>("Styles/DialogueGraph");
            styleSheets.Add(stylesheet);
            RegisterCallback<DragUpdatedEvent>(OnDragUpdated);
            RegisterCallback<DragPerformEvent>(OnDragPerformEvent);

            graphViewChanged = OnGraphViewChanged;
            serializeGraphElements += OnCutCopyOperation;
            unserializeAndPaste += OnPasteOperation;
        }

        private void OnDragPerformEvent(DragPerformEvent evt)
        {
            var selection = DragAndDrop.GetGenericData("DragSelection") as List<ISelectable>;
            IEnumerable<BlackboardField> fields = selection.OfType<BlackboardField>();
            foreach (var field in fields) 
                _variableFactory.Create(evt.mousePosition, field.text);
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
            
            return "hey";
        }

        private void OnPasteOperation(string operationName, string data)
        {
            ClearSelection();
            foreach (var element in _copyPaste.ElementsToCopy)
            {
                if (element is DialogueNodeView dialogueNodeView)
                {
                    var node = _factory.Copy(dialogueNodeView);
                    AddToSelection(node);
                }
            }
        }

        public void Initialize(DialogueNodeFactory factory, VariableNodeFactory variableFactory, ContextualMenuBuilder contextualMenuBuilder)
        {
            _factory = factory;
            _variableFactory = variableFactory;
            _contextualMenu = contextualMenuBuilder;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange) =>
            graphViewChange;

        private void OnMouseDown(MouseDownEvent evt)
        {
            if (evt.clickCount >= 2 && evt is { target: Edge edge })
            {
                var worldPosition = contentViewContainer.WorldToLocal(evt.mousePosition);
                _factory.CreateRedirectNode(worldPosition, edge, OnMouseDown);
            }
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) =>
            ports.ToList();

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt) =>
            _contextualMenu.BuildContextualMenu(evt, base.BuildContextualMenu);

        public void Find(DialogueNodeView view)
        {
            var viewPosition = view.GetPosition();
            CalculateFrameTransform(viewPosition, layout, 0, out var translation, out var scaling);
            UpdateViewTransform(translation, scaling);
            
            ClearSelection();
            AddToSelection(view);
        }

        public void Populate(DialogueGraph graph)
        {
            foreach (var node in graph.Nodes) 
                _factory.CreateFrom(node);
        }
    }
}