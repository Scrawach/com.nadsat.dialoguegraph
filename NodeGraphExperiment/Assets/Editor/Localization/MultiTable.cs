using System.Collections.Generic;
using System.Linq;
using Editor.AssetManagement;
using Runtime.Localization;

namespace Editor.Localization
{
    public class MultiTable
    {
        private readonly LanguageProvider _language;
        private readonly Dictionary<string, Table> _tables = new();
        private string _key;

        public MultiTable(LanguageProvider language) =>
            _language = language;

        public void Initialize(string key) =>
            _key = key;

        public string Create(string key)
        {
            if (!_tables.ContainsKey(key))
            {
                _tables[key] = new Table(key);
                _tables[key].AddHeader(_language.CurrentLanguage);
            }

            var table = _tables[key];
            var uniqueId = GenerateUniqueId(_key, key, table);
            table.AddRow(uniqueId);
            return uniqueId;
        }

        public void Update(string uniqueId, string value)
        {
            var key = GetKeyFromUniqueId(uniqueId);
            var table = _tables[key];
            table.Set(_language.CurrentLanguage, uniqueId, value);
        }

        public string Get(string uniqueId)
        {
            var key = GetKeyFromUniqueId(uniqueId);
            var table = _tables[key];

            if (!table.HasHeader(_language.CurrentLanguage))
                table.AddHeader(_language.CurrentLanguage);

            return table.Get(_language.CurrentLanguage, uniqueId);
        }

        public bool Remove(string uniqueId)
        {
            var key = GetKeyFromUniqueId(uniqueId);
            var table = _tables[key];
            return table.Remove(uniqueId);
        }

        public void AddHeaders(string[] headers)
        {
            foreach (var table in _tables.Values)
            foreach (var header in headers)
            {
                if (table.HasHeader(header))
                    continue;

                table.AddHeader(header);
            }
        }

        public IEnumerable<CsvInfo> ExportToCsv() =>
            _tables.Values
                .Select(table => table.ExportToCsv())
                .ToArray();

        public void ImportFromCsv(string[] names, CsvText[] csv)
        {
            for (var i = 0; i < names.Length; i++)
            {
                var table = new Table(names[i], csv[i]);
                _tables.Add(names[i], table);
            }
        }

        private static string GetKeyFromUniqueId(string uniqueId)
        {
            var items = uniqueId.Split(".");
            var key = items[1];
            return key;
        }

        private static string GenerateUniqueId(string firstKey, string secondKey, Table table)
        {
            var index = 0;
            var value = $"{firstKey}.{secondKey}.{index:D3}";

            while (table.ContainsKey(value))
            {
                index++;
                value = $"{firstKey}.{secondKey}.{index:D3}";
            }

            return value;
        }

        public void Clear() =>
            _tables.Clear();
    }
}