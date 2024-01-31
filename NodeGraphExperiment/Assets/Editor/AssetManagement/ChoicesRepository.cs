using System.Collections.Generic;
using System.Linq;

namespace Editor.AssetManagement
{
    public class ChoicesRepository
    {
        private readonly Dictionary<string, string> _content = new();
        
        private int _lastId;

        public string Create()
        {
            var id = GenerateUniqueChoiceId();
            _content[id] = "none";
            _lastId++;
            return id;
        }

        public string Get(string choiceId)
        {
            if (_content.ContainsKey(choiceId))
                return _content[choiceId];
            return "none";
        }
        
        public CsvTableInfo ExportToCsv() =>
            new CsvTableInfo()
            {
                Name = "Choices",
                Headers = "Keys,Russian",
                Lines = _content.Select(keyValuePair => $"\"{keyValuePair.Key}\",\"{keyValuePair.Value}\"").ToArray()
            };

        public string Update(string choiceId, string content) =>
            _content[choiceId] = content;

        private string GenerateUniqueChoiceId() =>
            $"LVL.BTN.{_lastId:D3}";
    }
}