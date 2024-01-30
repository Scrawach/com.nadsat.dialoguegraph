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
        
        public VisualElement Build(VisualElement target) =>
            target switch
            {
                DialogueNodeView dialogueView => CreateDialogueNodeInspector(dialogueView),
                ChoicesNodeView choicesView => new ChoicesNodeInspectorView(choicesView.Model, _choices),
                SwitchNodeView switchView => new SwitchNodeInspectorView(switchView.Model),
                _ => new VisualElement()
            };

        private VisualElement CreateDialogueNodeInspector(DialogueNodeView nodeView)
        {
            var inspector = new DialogueNodeInspectorView(nodeView.Model, _searchWindow, _phrases);
            inspector.UpdateDropdownChoices(_persons.All());
            return inspector;
        }
    }
}