using System;
using Editor.AssetManagement;
using Editor.Drawing.Nodes;
using Editor.Factories;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Editor
{
    public class ContextualMenuBuilder
    {
        private readonly PersonRepository _persons;
        private readonly DialogueNodeFactory _factory;

        private DialogueNodeView _entryPoint;
        
        public ContextualMenuBuilder(PersonRepository persons, DialogueNodeFactory factory)
        {
            _persons = persons;
            _factory = factory;
        }
        
        public void BuildContextualMenu(ContextualMenuPopulateEvent evt, Action<ContextualMenuPopulateEvent> onBaseContextualMenu = null)
        {
            if (evt.target is not GraphView)
            {
                if (evt.target is DialogueNodeView dialogueNodeView)
                {
                    evt.menu.AppendAction("Set as Root", _ =>
                    {
                        _entryPoint?.MarkAsRoot(false);
                        _entryPoint = dialogueNodeView;
                        _entryPoint.MarkAsRoot(true);
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