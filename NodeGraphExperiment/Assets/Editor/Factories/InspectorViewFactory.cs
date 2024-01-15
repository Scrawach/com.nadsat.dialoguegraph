using System;
using System.Linq;
using Editor.AssetManagement;
using Editor.Drawing.Inspector;
using Editor.Drawing.Nodes;
using Editor.Windows.Search;
using UnityEngine.UIElements;

namespace Editor.Factories
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
                var inspector = new DialogueNodeInspectorView(nodeView.Model, _searchWindow, _phrases);
                inspector.UpdateDropdownChoices(_personDatabase.Persons.Select(p => p.Name));
                return inspector;
            }

            throw new Exception("Invalid visual element!");
        }
    }
}