using System.IO;
using Editor.Importers;
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

        public void Export()
        {
            var csvInfos = _table.ExportToCsv();
            var path = Path.Combine(Application.dataPath, "Resources/Dialogues/Tutor");
            
            foreach (var csvInfo in csvInfos)
            {
                var pathToFile = Path.Combine(path, $"{csvInfo.Name}.csv");
                File.WriteAllText(pathToFile, csvInfo.Content);
            }
            
            AssetDatabase.Refresh();
        }
    }
}