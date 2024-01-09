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

        private readonly DialogueNodeViewFactory _factory;

        private DialoguePersonDatabase _personDatabase;
        
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

            _factory = new DialogueNodeViewFactory(this);
            graphViewChanged = OnGraphViewChanged;

            foreach (var item in TestItems()) 
                AddElement(item);
        }

        public void Initialize(DialoguePersonDatabase personDatabase) =>
            _personDatabase = personDatabase;

        private IEnumerable<DialogueNodeView> TestItems()
        {
            yield return _factory.From(NodeFactory.Elena("tutor_elena_001", "911, оператор службы спасения Елена. Чем могу помочь?"));
            var mark = NodeFactory.Mark("tutor_mark_001", "Приветствую, Елена! Это начальство беспокоит. Вы уже на рабочем месте?");
            mark.BackgroundImage = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/mark_001.jpg");
            yield return _factory.From(mark);
            yield return _factory.From(NodeFactory.Elena("tutor_elena_002", "Здравствуйте, мистер Уильямс. Так точно."));
            mark = NodeFactory.Mark("tutor_mark_002", "Дальше просто Марк, пожалуйста. Вы пришли раньше, чем нужно, взяли трубку сразу... Может, вам ещё и кофе здешний нравится? Ладно, шучу — его никто не любит.");
            mark.BackgroundImage = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/mark_002.jpg");
            yield return _factory.From(mark);
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

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            // base.BuildContextualMenu(evt);
            evt.menu.AppendAction("Create Node", _ => CreateNodeView());

            foreach (var personData in _personDatabase.Persons)
            {
                evt.menu.AppendAction($"Create {personData.Name}", _ => CreateFrom(personData));
            }
        }

        private void CreateFrom(DialoguePersonData data)
        {
            var viewData = new DialogueNodeViewData()
            {
                PersonName = data.Name,
                BackgroundColor = data.Color,
                Title = "none",
                Description = "none",
                Icon = data.Icon
            };
            
            AddElement(_factory.From(viewData));
        }

        private void CreateNodeView()
        {
            var dialogueNode = _factory.CreateDialogueNode();
            dialogueNode.OnNodeSelected += (node) => OnNodeSelected?.Invoke(node);
            AddElement(dialogueNode);
        }
    }
}