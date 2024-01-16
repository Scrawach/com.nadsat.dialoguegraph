using System;
using System.Collections.Generic;
using Editor.Drawing.Nodes;
using Editor.Factories;
using Editor.Serialization;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class DialogueGraphView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<DialogueGraphView, UxmlTraits> { }

        public event Action<DialogueNodeView> OnNodeSelected;
        public event Action<DialogueNodeView> OnNodeUnselected; 

        private DialogueNodeFactory _factory;
        private ContextualMenuBuilder _contextualMenu;
        private readonly CopyPasteNodes _copyPaste;

        public DialogueGraphView()
        {
            _copyPaste = new CopyPasteNodes();
            
            Insert(0, new GridBackground());

            var zoomer = new ContentZoomer();
            zoomer.maxScale = 2f;
            this.AddManipulator(zoomer);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            var stylesheet = Resources.Load<StyleSheet>("Styles/DialogueGraph");
            styleSheets.Add(stylesheet);

            graphViewChanged = OnGraphViewChanged;
            serializeGraphElements += OnCutCopyOperation;
            unserializeAndPaste += OnPasteOperation;
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
                    //var node = _factory.Copy(dialogueNodeView);
                    //AddToSelection(node);
                }
            }
        }

        public void Initialize(DialogueNodeFactory factory, ContextualMenuBuilder contextualMenuBuilder)
        {
            _factory = factory;
            _contextualMenu = contextualMenuBuilder;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {

            if (graphViewChange.elementsToRemove != null)
            {
                foreach (var element in graphViewChange.elementsToRemove)
                {
                    Debug.Log($"{element}");
                }
            }
            
            if (graphViewChange.edgesToCreate != null)
            {
                foreach (var edge in graphViewChange.edgesToCreate)
                {
                    edge.RegisterCallback<MouseDownEvent>(OnMouseDown);
                }
            }

            return graphViewChange;
        }

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
    }
}