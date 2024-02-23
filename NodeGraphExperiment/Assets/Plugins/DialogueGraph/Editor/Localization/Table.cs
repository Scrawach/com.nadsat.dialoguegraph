using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nadsat.DialogueGraph.Runtime.Localization;

namespace Nadsat.DialogueGraph.Editor.Localization
{
    public class Table
    {
        private readonly Dictionary<string, List<string>> _content = new();
        private List<string> _headers = new() {"Keys"};

        public Table(string name) =>
            Name = name;

        public Table(string name, CsvText csv) =>
            ImportFromCsv(name, csv);

        public string Name { get; private set; }

        public IEnumerable<string> Keys => _content.Keys;

        public string Get(string column, string row)
        {
            var columnIndex = _headers.IndexOf(column) - 1;
            var contentTable = _content[row];
            return contentTable[columnIndex];
        }

        public void Set(string column, string row, string content)
        {
            var columnIndex = _headers.IndexOf(column) - 1;
            var contentTable = _content[row];
            contentTable[columnIndex] = content;
        }

        public void AddHeader(string header)
        {
            _headers.Add(header);
            foreach (var contentValue in _content.Values)
                contentValue.Add(string.Empty);
        }

        public bool HasHeader(string header) =>
            _headers.Contains(header);

        public bool ContainsKey(string row) =>
            _content.ContainsKey(row);

        public bool Remove(string row) =>
            _content.Remove(row);

        public void AddRow(string row) =>
            _content[row] = _headers.Skip(1)
                .Select(_ => string.Empty).ToList();

        public CsvInfo ExportToCsv()
        {
            var builder = new StringBuilder();
            builder.Append(CreateCsvLine(_headers));

            foreach (var (key, items) in _content)
            {
                builder.Append(Environment.NewLine);
                builder.Append($"\"{key}\",");
                builder.Append(CreateCsvLine(items));
            }

            return new CsvInfo(Name, builder.ToString());
        }

        public void ImportFromCsv(string name, CsvText csv)
        {
            Name = name;
            _headers = csv.Rows().First().ToList();
            foreach (var content in csv.Rows().Skip(1))
            {
                var key = content[0];
                var values = content.Skip(1).ToList();
                _content[key] = values;
            }
        }

        private string CreateCsvLine(IReadOnlyList<string> items)
        {
            var builder = new StringBuilder();
            builder.Append($"\"{items.First()}\"");
            for (var i = 1; i < items.Count; i++)
                builder.Append($",\"{items[i]}\"");
            return builder.ToString();
        }

        public void Clear()
        {
            _content.Clear();
            _headers.Clear();
        }
    }
}