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

            _factory = new DialogueNodeViewFactory();
            graphViewChanged = OnGraphViewChanged;

            foreach (var item in TestItems()) 
                AddElement(item);
        }

        private IEnumerable<DialogueNodeView> TestItems()
        {
            yield return _factory.From(NodeFactory.Elena("tutor_elena_001", "911, оператор службы спасения Елена. Чем могу помочь?"));
            var mark = NodeFactory.Mark("tutor_mark_001", "Приветствую, Елена! Это начальство беспокоит. Вы уже на рабочем месте?");
            mark.PathToImage = "Assets/mark_001.jpg";
            mark.HasSound = true;
            yield return _factory.From(mark);
            yield return _factory.From(NodeFactory.Elena("tutor_elena_002", "Здравствуйте, мистер Уильямс. Так точно."));
            mark = NodeFactory.Mark("tutor_mark_002", "Дальше просто Марк, пожалуйста. Вы пришли раньше, чем нужно, взяли трубку сразу... Может, вам ещё и кофе здешний нравится? Ладно, шучу — его никто не любит.");
            mark.PathToImage = "Assets/mark_002.jpg";
            mark.HasError = true;
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
                CreateRedirectNode(evt.mousePosition, edge);
        }


        private void CreateRedirectNode(Vector2 position, Edge target)
        {
            var (redirectNode, input, output) = CreateNode2();
            position = contentViewContainer.WorldToLocal(position);
            redirectNode.SetPosition(new Rect(position, Vector2.zero));

            var edgeInput = target.input;
            var edgeOutput = target.output;
            
            target.input.Disconnect(target);
            target.output.Disconnect(target);
            target.Clear();

            var tempEdge1 = Connect(edgeInput, output);
            var tempEdge2 = Connect(input, edgeOutput);
            
            redirectNode.title = "";
            redirectNode.styleSheets.Add(Resources.Load<StyleSheet>("Styles/RedirectNode"));
            VisualElement contents = redirectNode.mainContainer.Q("contents");
            VisualElement divider = contents?.Q("divider");

            if (divider != null)
            {
                divider.RemoveFromHierarchy();
            }
            redirectNode.RefreshPorts();
            redirectNode.RefreshExpandedState();
            
            AddElement(redirectNode);
            AddElement(tempEdge1);
            AddElement(tempEdge2);
        }
        
        public Edge Connect(Port a, Port b)
        {
            var edge = new Edge()
            {
                input = a,
                output = b
            };
            
            edge.input.Connect(edge);
            edge.output.Connect(edge);
            edge.RegisterCallback<MouseDownEvent>(OnMouseDown);
            return edge;
        }
        
        public (Node, Port input, Port output) CreateNode2()
        {
            var dialogueNode = new RedirectNode()
            {
            };

            var port = dialogueNode.InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
            dialogueNode.inputContainer.Add(port);
            var port2 = dialogueNode.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
            dialogueNode.outputContainer.Add(port2);

            dialogueNode.RefreshPorts();
            dialogueNode.RefreshExpandedState();
            
            return (dialogueNode, port, port2);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList();
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            // base.BuildContextualMenu(evt);
            evt.menu.AppendAction("Create Node", _ => CreateNodeView());
        }

        private void CreateNodeView()
        {
            var dialogueNode = _factory.CreateDialogueNode();
            dialogueNode.OnNodeSelected += (node) => OnNodeSelected?.Invoke(node);
            AddElement(dialogueNode);
        }
    }
}