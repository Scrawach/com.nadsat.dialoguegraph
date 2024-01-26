using System;
using Editor.Drawing.Nodes;
using Runtime.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Factories
{
    public class RedirectNodeFactory
    {
        private readonly GraphView _canvas;
        private readonly INodeViewFactory _factory;

        public RedirectNodeFactory(GraphView canvas, INodeViewFactory factory)
        {
            _canvas = canvas;
            _factory = factory;
        }
        
        public RedirectNodeView CreateRedirect(Edge target, Vector2 position, EventCallback<MouseDownEvent> onMouseDown = null)
        {
            target.input.Disconnect(target);
            target.output.Disconnect(target);
            target.Clear();
            _canvas.RemoveElement(target);
            
            var redirectNode = new RedirectNode
            {
                Guid = Guid.NewGuid().ToString(), 
                Position = new Rect(_canvas.contentViewContainer.WorldToLocal(position), Vector2.zero)
            };
            var view = _factory.CreateRedirect(redirectNode);
            
            var leftEdge = CreateEdge(target.output, view.inputContainer[0] as Port, onMouseDown);
            var rightEdge = CreateEdge(target.input, view.outputContainer[0] as Port, onMouseDown);
            _canvas.AddElement(leftEdge);
            _canvas.AddElement(rightEdge);
            return view;
        }

        private static Edge CreateEdge(Port a, Port b, EventCallback<MouseDownEvent> onMouseDown = null)
        {
            var edge = a.ConnectTo(b);
            
            if (onMouseDown != null)
                edge.RegisterCallback(onMouseDown);
            
            return edge;
        }
    }
}