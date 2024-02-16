using System;
using System.Collections.Generic;
using System.Linq;
using Editor.Factories;
using Runtime.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.ContextualMenu
{
    public class NodesCreationMenuBuilder
    {
        private readonly Dictionary<string, Action<Vector2>> _builders;
        private readonly GraphView _graphView;
        private readonly INodeViewFactory _nodeViewFactory;
        private readonly TemplateDialogueFactory _templateFactory;

        public NodesCreationMenuBuilder(GraphView graphView, INodeViewFactory nodeViewFactory, TemplateDialogueFactory templateFactory)
        {
            _graphView = graphView;
            _nodeViewFactory = nodeViewFactory;
            _templateFactory = templateFactory;
            _builders = new Dictionary<string, Action<Vector2>>
            {
                ["Dialogue Node"] = position => nodeViewFactory.CreateDialogue(NewModel<DialogueNode>(position)),
                ["Choices Node"] = position => nodeViewFactory.CreateChoices(NewModel<ChoicesNode>(position)),
                ["Switch Node"] = position => nodeViewFactory.CreateSwitch(NewModel<SwitchNode>(position)),
                ["Variable Node"] = position => nodeViewFactory.CreateVariable(NewModel<VariableNode>(position))
            };
        }

        public void Build(ContextualMenuPopulateEvent evt)
        {
            BuildCreationMenu(evt);
            BuildTemplateNodes(evt);
        }

        private void BuildCreationMenu(ContextualMenuPopulateEvent evt)
        {
            foreach (var builder in _builders.Keys.Reverse())
                evt.menu.InsertAction(0, $"Create Node/{builder}", action => _builders[builder].Invoke(action.eventInfo.mousePosition));
        }

        private void BuildTemplateNodes(ContextualMenuPopulateEvent evt)
        {
            foreach (var template in _templateFactory.AvailableTemplates())
                evt.menu.InsertAction(1, $"Templates/{template}", action => { _templateFactory.Create(template, action.eventInfo.mousePosition); });
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