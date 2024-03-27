using Nadsat.DialogueGraph.Editor.AssetManagement;
using Nadsat.DialogueGraph.Editor.Audios;
using Nadsat.DialogueGraph.Editor.Data;
using Nadsat.DialogueGraph.Editor.Drawing.Inspector;
using Nadsat.DialogueGraph.Editor.Drawing.Nodes;
using Nadsat.DialogueGraph.Editor.Windows.Search;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Factories
{
    public class InspectorViewFactory
    {
        private readonly ChoicesRepository _choices;
        private readonly ExpressionVerifier _expressionVerifier;
        private readonly IAudioEditorService _audioService;
        private readonly DialogueDatabase _database;
        private readonly PhraseRepository _phrases;
        private readonly SearchWindowProvider _searchWindow;

        private DialogueNodeInspectorView _openedDialogueNodeInspector;

        public InspectorViewFactory(DialogueDatabase database, SearchWindowProvider searchWindow, PhraseRepository phrases, 
            ChoicesRepository choices, ExpressionVerifier expressionVerifier, IAudioEditorService audioService)
        {
            _database = database;
            _searchWindow = searchWindow;
            _phrases = phrases;
            _choices = choices;
            _expressionVerifier = expressionVerifier;
            _audioService = audioService;
        }

        public VisualElement Build(VisualElement target) =>
            target switch
            {
                DialogueNodeView dialogueView => CreateDialogueNodeInspector(dialogueView),
                ChoicesNodeView choicesView => new ChoicesNodeInspectorView(choicesView.Model, _choices),
                SwitchNodeView switchView => new SwitchNodeInspectorView(switchView.Model, _expressionVerifier, _searchWindow),
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