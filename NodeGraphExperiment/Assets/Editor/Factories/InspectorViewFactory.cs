using Editor.AssetManagement;
using Editor.Drawing.Inspector;
using Editor.Drawing.Nodes;
using Editor.Windows.Search;
using UnityEngine.UIElements;

namespace Editor.Factories
{
    public class InspectorViewFactory
    {
        private readonly DialogueDatabase _database;
        private readonly SearchWindowProvider _searchWindow;
        private readonly PhraseRepository _phrases;
        private readonly ChoicesRepository _choices;

        public InspectorViewFactory(DialogueDatabase database, SearchWindowProvider searchWindow, PhraseRepository phrases, ChoicesRepository choices)
        {
            _database = database;
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
            inspector.UpdateDropdownChoices(_database.All());
            return inspector;
        }
    }
}