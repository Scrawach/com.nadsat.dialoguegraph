using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nadsat.DialogueGraph.Runtime.Localization
{
    public class CsvAsset
    {
        private readonly string _path;

        public CsvAsset(string path) =>
            _path = path;

        public IEnumerable<string[]> Rows() => 
            Rows(Resources.Load<TextAsset>(_path));

        public static IEnumerable<string[]> Rows(TextAsset asset)
        {
            var rows = asset.text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            foreach (var row in rows)
                yield return CsvParser.FieldsFrom(row);
        }
    }
}