using Editor.Localization;

namespace Editor.AssetManagement
{
    public class ChoicesRepository
    {
        private readonly MultiTable _table;

        public ChoicesRepository(MultiTable table) =>
            _table = table;

        public string Create() =>
            _table.Create("Button");

        public string Get(string choiceId) =>
            _table.Get(choiceId);

        public void Update(string choiceId, string content) =>
            _table.Update(choiceId, content);

        public void Remove(string id) =>
            _table.Remove(id);
    }
}