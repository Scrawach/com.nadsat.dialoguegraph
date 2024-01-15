using System;
using System.Linq;
using UnityEngine.UIElements;

namespace Editor
{
    public class InspectorViewFactory
    {
        private readonly DialoguePersonDatabase _personDatabase;
        private readonly SearchWindowProvider _searchWindow;
        private readonly PhraseRepository _phrases;

        public InspectorViewFactory(DialoguePersonDatabase personDatabase, SearchWindowProvider searchWindow, PhraseRepository phrases)
        {
            _personDatabase = personDatabase;
            _searchWindow = searchWindow;
            _phrases = phrases;
        }

        public VisualElement Build(VisualElement target)
        {
            if (target is DialogueNodeView nodeView)
            {
                var inspector = new DialogueNodeInspectorView(nodeView.DialogueNode, _searchWindow, _phrases);
                inspector.Update(_personDatabase.Persons.Select(p => p.Name));
                return inspector;
            }

            throw new Exception("Invalid visual element!");
        }
    }
}