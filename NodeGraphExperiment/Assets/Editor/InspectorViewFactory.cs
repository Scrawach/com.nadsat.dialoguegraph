using System;
using System.Linq;
using UnityEngine.UIElements;

namespace Editor
{
    public class InspectorViewFactory
    {
        private readonly DialoguePersonDatabase _personDatabase;

        public InspectorViewFactory(DialoguePersonDatabase personDatabase) =>
            _personDatabase = personDatabase;
        
        public VisualElement Build(VisualElement target)
        {
            if (target is DialogueNodeView nodeView)
            {
                var inspector = new DialogueNodeInspectorView(nodeView.DialogueNode);
                inspector.Update(_personDatabase.Persons.Select(p => p.Name));
                return inspector;
            }

            throw new Exception("Invalid visual element!");
        }
    }
}