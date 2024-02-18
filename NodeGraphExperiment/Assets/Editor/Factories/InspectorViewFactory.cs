using Editor.AssetManagement;
using Editor.Audios;
using Editor.Drawing.Inspector;
using Editor.Drawing.Nodes;
using Editor.Windows.Search;
using UnityEngine.UIElements;

namespace Editor.Factories
{
    public class InspectorViewFactory
    {
        private readonly ChoicesRepository _choices;
        private readonly IAudioEditorService _audioService;
        private readonly DialogueDatabase _database;
        private readonly PhraseRepository _phrases;
        private readonly SearchWindowProvider _searchWindow;

        private DialogueNodeInspectorView _openedDialogueNodeInspector;

        public InspectorViewFactory(DialogueDatabase database, SearchWindowProvider searchWindow, PhraseRepository phrases, 
            ChoicesRepository choices, IAudioEditorService audioService)
        {
            _database = database;
            _searchWindow = searchWindow;
            _phrases = phrases;
            _choices = choices;
            _audioService = audioService;
        }

        public VisualElement Build(VisualElement target) =>
            target switch
            {
                DialogueNodeView dialogueView => CreateDialogueNodeInspector(dialogueView),
                ChoicesNodeView choicesView => new ChoicesNodeInspectorView(choicesView.Model, _choices),
                SwitchNodeView switchView => new SwitchNodeInspectorView(switchView.Model),
                AudioEventNodeView audioEventView => new AudioEventInspectorView(audioEventView.Model, _audioService, _searchWindow),
                _ => new VisualElement()
            };

        public void StartEditPhrase(string phraseId) =>
            _openedDialogueNodeInspector.StartEditPhrase();

        private VisualElement CreateDialogueNodeInspector(DialogueNodeView nodeView)
        {
            var inspector = new DialogueNodeInspectorView(nodeView.Model, _searchWindow, _phrases);
            _openedDialogueNodeInspector = inspector;
            inspector.UpdateDropdownChoices(_database.All());
            return inspector;
        }
    }
}