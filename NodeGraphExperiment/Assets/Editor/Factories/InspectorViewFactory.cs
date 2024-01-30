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
        private readonly ChoicesRepository _choices;

        public InspectorViewFactory(PersonRepository persons, SearchWindowProvider searchWindow, PhraseRepository phrases, ChoicesRepository choices)
        {
            _persons = persons;
            _searchWindow = searchWindow;
            _phrases = phrases;
            _choices = choices;
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
                var inspector = new ChoicesNodeInspectorView(choicesView.Model, _choices);
                return inspector;
            }
            else if (target is SwitchNodeView switchNodeView)
            {
                var inspector = new SwitchNodeInspectorView(switchNodeView.Model);
                return inspector;
            }

            return new VisualElement();
            throw new Exception("Invalid visual element!");
        }
    }
}