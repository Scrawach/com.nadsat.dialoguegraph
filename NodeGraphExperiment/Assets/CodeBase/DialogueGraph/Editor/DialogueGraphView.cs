using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeBase.DialogueGraph.Editor
{
    public class DialogueGraphView : GraphView
    {
        public void Initialize()
        {
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            AddElement(CreateNode());
            AddElement(CreateNode());

            graphViewChanged = OnGraphViewChanged;
        }

        private void Update()
        {
            Debug.Log("TIck");
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
            Debug.Log($"{position}");
            
            var previousA = target.input;
            var previousB = target.output;
            
            previousA.Disconnect(target);
            previousB.Disconnect(target);

            var a = new Edge();

            var (node, input, output) = CreateNode2();
            node.SetPosition(new Rect(position, Vector2.one));
            input.Connect(a);
            previousA.Connect(a);
            
            var b = new Edge();
            output.Connect(b);
            previousB.Connect(b);
            
            AddElement(node);
            AddElement(a);
            AddElement(b);

            target.Clear();
        }
        
        public Node CreateEmptyNode()
        {
            var node = new DialogueNode();
            var input = node.InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float));
            var output = node.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
            node.mainContainer.Add(input);
            node.expanded = false;
            
            VisualElement contents = node.mainContainer.Q("contents");
            VisualElement divider = contents?.Q("divider");
            if (divider != null)
            {
                Debug.Log(divider);
                divider.RemoveFromHierarchy();
            }
            node.RefreshPorts();
            node.RefreshExpandedState();
            return node;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList();
        }

        public Node CreateNode()
        {
            var dialogueNode = new DialogueNode()
            {
                Guid = Guid.NewGuid().ToString()
            };

            var port = dialogueNode.InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float));
            port.name = "input";
            dialogueNode.inputContainer.Add(port);
            port = dialogueNode.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
            port.name = "output";
            dialogueNode.outputContainer.Add(port);

            dialogueNode.RefreshPorts();
            dialogueNode.RefreshExpandedState();
            
            return dialogueNode;
        }
        
        public (Node, Port input, Port output) CreateNode2()
        {
            var dialogueNode = new DialogueNode()
            {
                Guid = Guid.NewGuid().ToString()
            };

            var port = dialogueNode.InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float));
            port.name = "input";
            dialogueNode.inputContainer.Add(port);
            var port2 = dialogueNode.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
            port2.name = "output";
            dialogueNode.outputContainer.Add(port2);

            dialogueNode.RefreshPorts();
            dialogueNode.RefreshExpandedState();
            
            return (dialogueNode, port, port2);
        }
    }
}