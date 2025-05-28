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
                .Select(CsvParser.FieldsFrom);
    }
}