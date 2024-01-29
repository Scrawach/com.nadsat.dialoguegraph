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
        private readonly PersonRepository _persons;
        private readonly SearchWindowProvider _searchWindow;
        private readonly PhraseRepository _phrases;

        public InspectorViewFactory(PersonRepository persons, SearchWindowProvider searchWindow, PhraseRepository phrases)
        {
            _persons = persons;
            _searchWindow = searchWindow;
            _phrases = phrases;
        }

        public VisualElement Build(VisualElement target)
        {
            if (target is DialogueNodeView nodeView)
            {
                var inspector = new DialogueNodeInspectorView(nodeView.Model, _searchWindow, _phrases);
                inspector.UpdateDropdownChoices(_persons.All());
                return inspector;
            }
            else if (target is ChoicesNodeView choicesView)
            {
                var inspector = new ChoicesNodeInspectorView(choicesView.Model);
                return inspector;
            }

            throw new Exception("Invalid visual element!");
        }
    }
}