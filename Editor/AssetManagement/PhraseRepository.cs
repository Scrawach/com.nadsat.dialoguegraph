using Nadsat.DialogueGraph.Editor.Localization;

namespace Nadsat.DialogueGraph.Editor.AssetManagement
{
    public class PhraseRepository
    {
        private readonly MultiTable _table;

        public PhraseRepository(MultiTable table) =>
            _table = table;

        public string Create(string personId) =>
            _table.Create(personId);

        public string Get(string phraseId) =>
            _table.Get(phraseId);

        public void Update(string phraseId, string value) =>
            _table.Update(phraseId, value);

        public bool Remove(string phraseId) =>
            _table.Remove(phraseId);
    }
}