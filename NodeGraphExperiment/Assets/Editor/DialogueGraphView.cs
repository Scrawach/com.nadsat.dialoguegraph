using System;
using System.Collections.Generic;
using System.Text;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class DialogueGraphView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<DialogueGraphView, GraphView.UxmlTraits> { }

        public event Action<DialogueNodeView> OnNodeSelected;
        public event Action<DialogueNodeView> OnNodeUnselected; 

        private DialogueNodeViewFactory _factory;
        private ContextualMenuBuilder _contextualMenu;
        private CopyPasteNodes _copyPaste;

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
            
            var stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/DialogueGraph.uss");
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
                    var node = _factory.Copy(dialogueNodeView);
                    AddToSelection(node);
                }
            }
        }

        public void Initialize(DialogueNodeViewFactory factory, ContextualMenuBuilder contextualMenuBuilder)
        {
            _factory = factory;
            _contextualMenu = contextualMenuBuilder;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
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
            if (evt is { target: Edge edge })
            {
                var worldPosition = contentViewContainer.WorldToLocal(evt.mousePosition);
                _factory.CreateRedirectNode(worldPosition, edge, OnMouseDown);
            }
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList();
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt) =>
            _contextualMenu.BuildContextualMenu(this, evt);

        public void AddNode(DialogueNodeView nodeView)
        {
            nodeView.OnNodeSelected += (node) => OnNodeSelected?.Invoke(node);
            nodeView.OnNodeUnselected += (node) => OnNodeUnselected?.Invoke(node);
            AddElement(nodeView);
        }
    }
}