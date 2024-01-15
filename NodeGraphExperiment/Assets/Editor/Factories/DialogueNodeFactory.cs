using System;
using Editor.AssetManagement;
using Editor.Drawing.Nodes;
using Runtime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Factories
{
    public class DialogueNodeFactory
    {
        private readonly PersonRepository _persons;
        private readonly PhraseRepository _phrases;
        private readonly EditorAssets _assets;
        private readonly GraphView _canvas;

        public DialogueNodeFactory(PersonRepository persons, PhraseRepository phrases, EditorAssets assets, GraphView canvas)
        {
            _persons = persons;
            _phrases = phrases;
            _assets = assets;
            _canvas = canvas;
        }

        public void CreatePersonNode(string person, Vector2 position) =>
            CreateFrom(new DialogueNode()
            {
                Guid = Guid.NewGuid().ToString(), 
                PersonId = person, 
                Position = new Rect(position, Vector2.zero)
            });

        public DialogueNodeView CreateFrom(DialogueNode data)
        {
            var view = new DialogueNodeView(_phrases, _persons, _assets);
            view.Bind(data);
            CreatePortsFor(view);
            _canvas.AddElement(view);
            return view;
        }
        
        public void CreateGroup(Vector2 at)
        {
            var group = new Group();
            var worldPosition = _canvas.contentViewContainer.WorldToLocal(at);
            group.SetPosition(new Rect(worldPosition, Vector2.zero));
            _canvas.AddElement(group);
        }
        
        public RedirectNodeView CreateRedirectNode(Vector2 position, Edge target, EventCallback<MouseDownEvent> onMouseDown = null)
        {
            target.input.Disconnect(target);
            target.output.Disconnect(target);
            target.Clear();
            _canvas.RemoveElement(target);

            var redirectNode = new RedirectNodeView() { title = "" };
            
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
            input.portName = "";
            node.inputContainer.Add(input);
            var output = node.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
            node.outputContainer.Add(output);
            output.portName = "";
            
            node.RefreshPorts();
            node.RefreshExpandedState();
            return (input, output);
        }
    }
}