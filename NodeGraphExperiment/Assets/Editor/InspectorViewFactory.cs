using System;
using System.Linq;
using UnityEngine.UIElements;

namespace Editor
{
    public class InspectorViewFactory
    {
        private readonly DialoguePersonDatabase _personDatabase;
        private readonly SearchWindowProvider _searchWindow;

        public InspectorViewFactory(DialoguePersonDatabase personDatabase, SearchWindowProvider searchWindow)
        {
            _personDatabase = personDatabase;
            _searchWindow = searchWindow;
        }

        public VisualElement Build(VisualElement target)
        {
            if (target is DialogueNodeView nodeView)
            {
                var inspector = new DialogueNodeInspectorView(nodeView.DialogueNode, _searchWindow);
                inspector.Update(_personDatabase.Persons.Select(p => p.Name));
                return inspector;
            }

            throw new Exception("Invalid visual element!");
        }
    }
}