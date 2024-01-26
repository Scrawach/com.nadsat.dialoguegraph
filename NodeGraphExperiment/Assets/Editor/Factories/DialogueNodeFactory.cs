using System;
using System.Collections.Generic;
using Editor.AssetManagement;
using Editor.Drawing.Nodes;
using Editor.Factories.NodeListeners;
using Editor.Undo;
using Editor.Undo.Commands;
using Runtime.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Factories
{
    public class DialogueNodeFactory
    {
        private readonly PersonRepository _persons;
        private readonly PhraseRepository _phrases;
        private readonly IDialogueNodeListener _listener;
        private readonly GraphView _canvas;
        private readonly IUndoRegister _undoRegister;

        public DialogueNodeFactory(PersonRepository persons, PhraseRepository phrases, IDialogueNodeListener listener, 
            GraphView canvas, IUndoRegister undoRegister)
        {
            _persons = persons;
            _phrases = phrases;
            _listener = listener;
            _canvas = canvas;
            _undoRegister = undoRegister;
        }

        public void CreatePersonNode(string person, Vector2 position) =>
            CreateFrom(new DialogueNode()
            {
                Guid = Guid.NewGuid().ToString(), 
                PersonId = person, 
                Position = new Rect(_canvas.contentViewContainer.WorldToLocal(position), Vector2.zero)
            });

        public DialogueNodeView CreateWithoutUndo(DialogueNode data)
        {
            var view = new DialogueNodeView(_phrases, _persons);
            CreatePortsFor(view);

            view.Bind(data);
            view.SetPosition(data.Position);
            _listener.Register(view);
            _canvas.AddElement(view);
            return view;
        }
        
        public DialogueNodeView CreateFrom(DialogueNode data)
        {
            var view = CreateWithoutUndo(data);
            _undoRegister.Register(new AddElement(view, _canvas));
            return view;
        }
        
        public Group CreateGroup(Vector2 at)
        {
            var group = new Group();
            var worldPosition = _canvas.contentViewContainer.WorldToLocal(at);
            group.SetPosition(new Rect(worldPosition, Vector2.zero));
            _canvas.AddElement(group);
            return group;
        }

        public StickyNote CreateStickyNote(Vector2 at)
        {
            var stickyNote = new StickyNote();
            var worldPosition = _canvas.contentViewContainer.WorldToLocal(at);
            stickyNote.SetPosition(new Rect(worldPosition, Vector2.zero));
            _canvas.AddElement(stickyNote);
            return stickyNote;
        }

        public RedirectNodeView CreateRedirectNode(Vector2 position, Edge target, EventCallback<MouseDownEvent> onMouseDown = null)
        {
            target.input.Disconnect(target);
            target.output.Disconnect(target);
            target.Clear();
            _canvas.RemoveElement(target);

            var redirectNode = new RedirectNodeView
            {
                title = "",
                Model = new RedirectNode()
                {
                    Guid = Guid.NewGuid().ToString()
                }
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
            input.portName = "";
            node.inputContainer.Add(input);
            var output = node.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
            node.outputContainer.Add(output);
            output.portName = "";
            
            node.RefreshPorts();
            node.RefreshExpandedState();
            return (input, output);
        }

        public DialogueNodeView Copy(DialogueNodeView dialogueNodeView)
        {
            var newDialogueNode = new DialogueNode()
            {
                Guid = Guid.NewGuid().ToString(),
                PersonId = dialogueNodeView.Model.PersonId,
                Position = dialogueNodeView.Model.Position,
                PhraseId = dialogueNodeView.Model.PhraseId,
                PathToImage = dialogueNodeView.Model.PathToImage
            };
            return CreateFrom(newDialogueNode);
        }
    }
}