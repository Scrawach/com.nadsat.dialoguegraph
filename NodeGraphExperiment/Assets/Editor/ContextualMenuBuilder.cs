using System;
using Editor.AssetManagement;
using Editor.Drawing.Nodes;
using Editor.Factories;
using Editor.Factories.NodeListeners;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Editor
{
    public class ContextualMenuBuilder
    {
        private readonly PersonRepository _persons;
        private readonly NodesProvider _provider;
        private readonly ElementsFactory _factory;
        private readonly PersonTemplateFactory _templateFactory;

        public ContextualMenuBuilder(PersonRepository persons, NodesProvider provider, ElementsFactory factory, PersonTemplateFactory templateFactory)
        {
            _persons = persons;
            _provider = provider;
            _factory = factory;
            _templateFactory = templateFactory;
        }
        
        public void BuildContextualMenu(ContextualMenuPopulateEvent evt, Action<ContextualMenuPopulateEvent> onBaseContextualMenu = null)
        {
            if (evt.target is not GraphView)
            {
                if (evt.target is DialogueNodeView nodeView)
                {
                    evt.menu.AppendAction("Set as Root", _ =>
                    {
                        _provider.MarkAsRootNode(nodeView);
                    });
                    
                    evt.menu.AppendSeparator();
                }
                onBaseContextualMenu?.Invoke(evt);
                return;
            }
            
            evt.menu.AppendAction("Create Group", (action) => _factory.CreateGroup(at: action.eventInfo.mousePosition));
            evt.menu.AppendAction("Create Sticky Note", (action) => _factory.CreateStickyNote(at: action.eventInfo.mousePosition));
            evt.menu.AppendSeparator();
            
            foreach (var person in _persons.All())
            {
                evt.menu.AppendAction($"Templates/{person}", (action) =>
                    {
                        _templateFactory.CreateDialogue(person, action.eventInfo.mousePosition);
                    });
            }
            
            evt.menu.AppendSeparator();
            onBaseContextualMenu?.Invoke(evt);
        }
    }
}