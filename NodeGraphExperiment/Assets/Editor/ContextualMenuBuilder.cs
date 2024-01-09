using UnityEngine.UIElements;

namespace Editor
{
    public class ContextualMenuBuilder
    {
        private readonly DialoguePersonDatabase _personDatabase;
        private readonly DialogueNodeViewFactory _factory;

        public ContextualMenuBuilder(DialoguePersonDatabase personDatabase, DialogueNodeViewFactory factory)
        {
            _personDatabase = personDatabase;
            _factory = factory;
        }
        
        public void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            foreach (var personData in _personDatabase.Persons)
            {
                evt.menu.AppendAction($"Templates/{personData.Name}", 
                    (action) => _factory.CreatePersonNode(personData, position: action.eventInfo.mousePosition));
            }
        }
    }
}