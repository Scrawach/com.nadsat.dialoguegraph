using System;
using System.Collections.Generic;
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
        private DialoguePersonDatabase _personDatabase;
        private ContextualMenuBuilder _contextualMenu;

        public DialogueGraphView()
        {
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
        }

        public void Initialize(DialoguePersonDatabase personDatabase, PhraseRepository phraseRepository)
        {
            _personDatabase = personDatabase;
            _factory = new DialogueNodeViewFactory(this, personDatabase, phraseRepository);
            _contextualMenu = new ContextualMenuBuilder(personDatabase, _factory);
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
            _contextualMenu.BuildContextualMenu(evt);

        public void AddNode(DialogueNodeView nodeView)
        {
            nodeView.OnNodeSelected += (node) => OnNodeSelected?.Invoke(node);
            nodeView.OnNodeUnselected += (node) => OnNodeUnselected?.Invoke(node);
            AddElement(nodeView);
        }
    }
}