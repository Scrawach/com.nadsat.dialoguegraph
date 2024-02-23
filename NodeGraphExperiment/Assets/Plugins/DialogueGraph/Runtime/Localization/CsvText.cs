using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nadsat.DialogueGraph.Runtime.Localization
{
    public class CsvText
    {
        private readonly string _csv;

        public CsvText(string csv) =>
            _csv = csv;

        public IEnumerable<string[]> Rows() =>
            _csv.Split(Environment.NewLine)
                .Select(CsvLineFieldsFrom);

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