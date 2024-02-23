using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Nadsat.DialogueGraph.Runtime.Localization
{
    public class CsvAsset
    {
        private readonly string _path;

        public CsvAsset(string path) =>
            _path = path;

        public IEnumerable<string[]> Rows()
        {
            var table = Resources.Load<TextAsset>(_path);
            var rows = table.text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            foreach (var row in rows)
                yield return CsvLineFieldsFrom(row);
        }

        private static string[] CsvLineFieldsFrom(string row)
        {
            const char fieldDelimiter = ',';
            const char shieldDelimiter = '\"';

            var fields = new List<string>();
            var field = new StringBuilder();
            var beenShielded = false;
            foreach (var symbol in row)
                switch (symbol)
                {
                    case fieldDelimiter when !beenShielded:
                        fields.Add(field.ToString());
                        field = new StringBuilder();
                        break;
                    case shieldDelimiter:
                        beenShielded = !beenShielded;
                        break;
                    default:
                        field.Append(symbol);
                        break;
                }

            fields.Add(field.ToString());
            return fields.ToArray();
        }
    }
}