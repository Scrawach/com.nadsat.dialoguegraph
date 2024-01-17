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
        private readonly DialogueNodeFactory _factory;
        
        public ContextualMenuBuilder(PersonRepository persons, NodesProvider provider, DialogueNodeFactory factory)
        {
            _persons = persons;
            _provider = provider;
            _factory = factory;
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
            
            //evt.menu.AppendAction("Create Group", (action) => _factory.CreateGroup(at: action.eventInfo.mousePosition));
            
            foreach (var person in _persons.All())
            {
                evt.menu.AppendAction($"Templates/{person}", (action) =>
                    {
                        _factory.CreatePersonNode(person, position: action.eventInfo.mousePosition);
                    });
            }
        }
    }
}