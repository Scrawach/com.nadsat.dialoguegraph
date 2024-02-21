using System.IO;
using Editor.Data;
using Editor.Localization;
using UnityEditor;
using UnityEngine;

namespace Editor.Exporters
{
    public class CsvExporter
    {
        private readonly MultiTable _table;

        public CsvExporter(MultiTable table) => 
            _table = table;

        public void Export(string pathToFolder)
        {
            var csvInfos = _table.ExportToCsv();

            foreach (var csvInfo in csvInfos)
            {
                var pathToFile = Path.Combine(pathToFolder, $"{csvInfo.Name}.csv");
                File.WriteAllText(pathToFile, csvInfo.Content);
            }

            AssetDatabase.Refresh();
        }
    }
}