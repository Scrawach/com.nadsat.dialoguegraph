using Nadsat.DialogueGraph.Editor.AssetManagement;
using Nadsat.DialogueGraph.Editor.Drawing.Nodes;
using Nadsat.DialogueGraph.Runtime.Nodes;

namespace Nadsat.DialogueGraph.Editor.Factories
{
    public class DialogueNodeViewFactory
    {
        private readonly GenericNodeViewFactory _baseFactory;
        private readonly InspectorViewFactory _inspectorFactory;
        private readonly DialogueDatabase _persons;
        private readonly PhraseRepository _phrases;

        public DialogueNodeViewFactory(GenericNodeViewFactory baseFactory, DialogueDatabase persons, PhraseRepository phrases,
            InspectorViewFactory inspectorFactory)
        {
            _baseFactory = baseFactory;
            _persons = persons;
            _phrases = phrases;
            _inspectorFactory = inspectorFactory;
        }

        public DialogueNodeView Create(DialogueNode node)
        {
            var view = new DialogueNodeView(_phrases, _persons);
            view = _baseFactory.Create(view, node);

            if (IsPersonWithoutPhrase(node))
            {
                node.SetPhraseId(_phrases.Create(node.PersonId));
                _inspectorFactory.StartEditPhrase(node.PhraseId);
            }

            return view;
        }

        private static bool IsPersonWithoutPhrase(DialogueNode node) =>
            !string.IsNullOrWhiteSpace(node.PersonId) && string.IsNullOrWhiteSpace(node.PhraseId);
    }
}