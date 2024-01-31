using System;
using System.Collections.Generic;
using System.Linq;

namespace Editor.AssetManagement
{
    public class CsvTableInfo
    {
        public string Name;
        public string Headers;
        public string[] Lines;
    }

    public class TableRepository
    {
        private readonly Dictionary<string, string> _content = new();
        private string _name;

        public TableRepository(string name) =>
            _name = name;

        public string Create(string value = "")
        {
            var id = GenerateUniqueId(_name);
            _content[id] = value;
            return id;
        }

        public string Get(string key)
        {
            if (_content.ContainsKey(key))
                return _content[key];
            return "none";
        }

        public CsvTableInfo ExportToCsv() =>
            new CsvTableInfo()
            {
                Name = _name,
                Headers = "Keys,Russian",
                Lines = _content.Select(keyValuePair => $"\"{keyValuePair.Key}\",\"{keyValuePair.Value}\"").ToArray()
            };

        public void Import(CsvTableInfo info)
        {
            _name = info.Name;
            
        }

        public void Remove(string phraseId) =>
            _content.Remove(phraseId);

        public string GenerateUniqueId(string personId)
        {
            var index = 0;
            foreach (var key in _content.Keys)
            {
                var id = $"LVL.{personId}.{index:D3}";
                if (key != id)
                    return id;
                index++;
            }

            return $"LVL.{personId}.{index:D3}";
        }

        public bool ContainsKey(string phraseId) =>
            _content.ContainsKey(phraseId);

        public void Update(string phraseId, string content) =>
            _content[phraseId] = content;
    }
}