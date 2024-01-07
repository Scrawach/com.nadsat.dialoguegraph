using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class DialogueNodeViewFactory
    {
        private readonly DialogueGraphView _canvas;

        public DialogueNodeViewFactory(DialogueGraphView canvas) =>
            _canvas = canvas;

        public RedirectNode CreateRedirectNode(Vector2 position, Edge target, EventCallback<MouseDownEvent> onMouseDown = null)
        {
            target.input.Disconnect(target);
            target.output.Disconnect(target);
            target.Clear();
            _canvas.RemoveElement(target);

            var redirectNode = new RedirectNode
            {
                title = ""
            };
            
            redirectNode.styleSheets.Add(Resources.Load<StyleSheet>("Styles/RedirectNode"));

            var (input, output) = CreatePortsFor(redirectNode);
            redirectNode.SetPosition(new Rect(position, Vector2.zero));
            
            var leftEdge = CreateEdge(target.output, input, onMouseDown);
            var rightEdge = CreateEdge(target.input, output, onMouseDown);
                        
            _canvas.AddElement(redirectNode);
            _canvas.AddElement(leftEdge);
            _canvas.AddElement(rightEdge);
            return redirectNode;
        }

        private static Edge CreateEdge(Port a, Port b, EventCallback<MouseDownEvent> onMouseDown = null)
        {
            var edge = a.ConnectTo(b);
            
            if (onMouseDown != null)
                edge.RegisterCallback(onMouseDown);
            
            return edge;
        }

        private static (Port input, Port output) CreatePortsFor(Node node)
        {
            var input = node.InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
            node.inputContainer.Add(input);
            var output = node.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
            node.outputContainer.Add(output);
            
            node.RefreshPorts();
            node.RefreshExpandedState();
            return (input, output);
        }
        
        public DialogueNodeView CreateDialogueNode() =>
            From(NodeFactory.Elena("test", "description"));

        public DialogueNodeView From(DialogueNodeViewData data)
        {
            var node = new DialogueNodeView(data);
            node.Guid = Guid.NewGuid().ToString();

            var input = node.InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
            input.portName = "";
            node.inputContainer.Add(input);

            var output = node.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
            output.portName = "";
            node.outputContainer.Add(output);
            
            node.RefreshPorts();
            node.RefreshExpandedState();
            return node;
        }
    }
}