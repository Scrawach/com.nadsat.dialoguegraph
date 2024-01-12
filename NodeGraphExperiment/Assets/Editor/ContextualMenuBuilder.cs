using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Editor
{
    public class ContextualMenuBuilder
    {
        private readonly DialoguePersonDatabase _personDatabase;
        private readonly DialogueNodeViewFactory _factory;

        private DialogueNodeView _entryPoint;
        
        public ContextualMenuBuilder(DialoguePersonDatabase personDatabase, DialogueNodeViewFactory factory)
        {
            _personDatabase = personDatabase;
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
                        _entryPoint?.ResetEntryNode();
                        _entryPoint = dialogueNodeView;
                        _entryPoint.SetAsEntryNode();
                    });
                    
                    evt.menu.AppendSeparator();
                }
                onBaseContextualMenu?.Invoke(evt);
                return;
            }
            
            evt.menu.AppendAction("Create Group", (action) => _factory.CreateGroup(at: action.eventInfo.mousePosition));
            
            foreach (var personData in _personDatabase.Persons)
            {
                evt.menu.AppendAction($"Templates/{personData.Name}", 
                    (action) => _factory.CreatePersonNode(personData, position: action.eventInfo.mousePosition));
            }
        }
    }
}