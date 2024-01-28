using System;
using System.Collections.Generic;
using Editor.AssetManagement;
using Runtime.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Factories
{
    public class NodesCreationMenuBuilder
    {
        private readonly GraphView _graphView;
        private readonly INodeViewFactory _nodeViewFactory;
        private readonly Dictionary<string, Action<Vector2>> _builders;
        private readonly PersonRepository _persons;

        public NodesCreationMenuBuilder(GraphView graphView, INodeViewFactory nodeViewFactory, PersonRepository persons)
        {
            _graphView = graphView;
            _nodeViewFactory = nodeViewFactory;
            _persons = persons;
            _builders = new Dictionary<string, Action<Vector2>>()
            {
                ["Dialogue Node"] = (position) => nodeViewFactory.CreateDialogue(NewModel<DialogueNode>(position)),
                ["Choices Node"] = (position) => nodeViewFactory.CreateChoices(NewModel<ChoicesNode>(position)),
                ["Switch Node"] = (position) => nodeViewFactory.CreateSwitch(NewModel<SwitchNode>(position)),
                ["Variable Node"] = (position) => nodeViewFactory.CreateChangeVariable(NewModel<ChangeVariableNode>(position)),
            };
        }

        public void Build(ContextualMenuPopulateEvent evt)
        {
            BuildCreationMenu(evt);
            BuildTemplateNodes(evt);
        }

        private void BuildCreationMenu(ContextualMenuPopulateEvent evt)
        {
            foreach (var builder in _builders.Keys)
                evt.menu.AppendAction($"Create Node/{builder}", action => _builders[builder].Invoke(action.eventInfo.mousePosition));
        }

        private void BuildTemplateNodes(ContextualMenuPopulateEvent evt)
        {
            foreach (var personId in _persons.All())
                evt.menu.AppendAction($"Templates/{personId}", (action) =>
                {
                    _nodeViewFactory.CreateDialogue(DialogueForPerson(personId, action.eventInfo.mousePosition));
                });
        }

        private DialogueNode DialogueForPerson(string personId, Vector2 position)
        {
            var model = NewModel<DialogueNode>(position);
            model.PersonId = personId;
            return model;
        }

        private TModel NewModel<TModel>(Vector2 position) where TModel : BaseDialogueNode, new()
        {
            var model = new TModel
            {
                Guid = Guid.NewGuid().ToString(),
                Position = new Rect(_graphView.contentViewContainer.WorldToLocal(position), Vector2.zero)
            };
            return model;
        }
    }
}