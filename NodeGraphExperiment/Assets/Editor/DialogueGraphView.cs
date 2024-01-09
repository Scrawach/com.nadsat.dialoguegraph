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

        public void Initialize(DialoguePersonDatabase personDatabase)
        {
            _personDatabase = personDatabase;
            _factory = new DialogueNodeViewFactory(this, personDatabase);
            _contextualMenu = new ContextualMenuBuilder(personDatabase, _factory);
        }

        private IEnumerable<DialogueNodeView> TestItems()
        {
            yield return _factory.From(NodeFactory.Elena("tutor_elena_001", "911, оператор службы спасения Елена. Чем могу помочь?"));
            var mark = NodeFactory.Mark("tutor_mark_001", "Приветствую, Елена! Это начальство беспокоит. Вы уже на рабочем месте?");
            mark.BackgroundImage = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/mark_001.jpg");
            yield return _factory.From(mark);
            yield return _factory.From(NodeFactory.Elena("tutor_elena_002", "Здравствуйте, мистер Уильямс. Так точно."));
            mark = NodeFactory.Mark("tutor_mark_002", "Дальше просто Марк, пожалуйста. Вы пришли раньше, чем нужно, взяли трубку сразу... Может, вам ещё и кофе здешний нравится? Ладно, шучу — его никто не любит.");
            mark.BackgroundImage = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/mark_002.jpg");
            var node = _factory.From(mark);
            yield return node;
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