using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeBase.DialogueGraph.Editor
{
    public class DialogueGraphView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<DialogueGraphView, GraphView.UxmlTraits> { }

        public void Initialize()
        {
            Insert(0, new GridBackground());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            AddElement(CreateNode());
            AddElement(CreateNode());
            AddElement(CreateNode());

            graphViewChanged = OnGraphViewChanged;

            //var stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/DialogueGraph.uss");
            //styleSheets.Add(stylesheet);
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

            var port = dialogueNode.InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
            port.name = "input";
            dialogueNode.inputContainer.Add(port);
            port = dialogueNode.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
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

            var port = dialogueNode.InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
            dialogueNode.inputContainer.Add(port);
            var port2 = dialogueNode.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
            dialogueNode.outputContainer.Add(port2);

            dialogueNode.RefreshPorts();
            dialogueNode.RefreshExpandedState();
            
            return (dialogueNode, port, port2);
        }
    }
}