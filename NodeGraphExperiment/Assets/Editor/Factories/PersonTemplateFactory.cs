using System;
using Editor.Drawing.Nodes;
using Runtime.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Factories
{
    public class PersonTemplateFactory
    {
        private readonly INodeViewFactory _factory;
        private readonly GraphView _graphView;

        public PersonTemplateFactory(INodeViewFactory factory, GraphView graphView)
        {
            _factory = factory;
            _graphView = graphView;
        }

        public DialogueNodeView CreateDialogue(string personId, Vector2 position) =>
            _factory.CreateDialogue(new DialogueNode
            {
                Guid = Guid.NewGuid().ToString(),
                PersonId = personId,
                Position = new Rect(_graphView.contentViewContainer.WorldToLocal(position), Vector2.zero)
            });
    }
}