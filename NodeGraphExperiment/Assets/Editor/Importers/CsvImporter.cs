using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Editor.AssetManagement;
using UnityEngine;

namespace Editor.Importers
{
    public class CsvImporter
    {
        private readonly PhraseRepository _phrases;
        private readonly ChoicesRepository _choices;

        public CsvImporter(PhraseRepository phrases, ChoicesRepository choices)
        {
            _phrases = phrases;
            _choices = choices;
        }
        
        public void Import()
        {
            const string tableExtensions = ".csv";
            var path = Path.Combine(Application.dataPath, "Resources/Dialogues/Tutor");
            var filesInDirectory = Directory.GetFiles(path);
            foreach (var filepath in filesInDirectory)
            {
                Debug.Log($"{Path.GetExtension(filepath)}");
                if (Path.GetExtension(filepath) == tableExtensions) 
                    ImportCsv(File.ReadAllText(filepath));
            }
        }

        public CsvTableInfo ImportCsv(string csvContent)
        {
            var rows = csvContent.Split(Environment.NewLine);

            var info = new CsvTableInfo();
            var headers = CsvLineFieldsFrom(rows.First()).ToList();
            
            foreach (var row in rows.Skip(1))
            {
                foreach (var item in CsvLineFieldsFrom(row))
                {
                    Debug.Log($"{item}");
                }
            }
            return null;
        }

        private static string[] CsvLineFieldsFrom(string row)
        {
            const char fieldDelimiter = ',';
            const char shieldDelimiter = '\"';
            
            var fields = new List<string>();
            var field = new StringBuilder();
            var beenShielded = false;
            foreach (var symbol in row)
            {
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
            }

            fields.Add(field.ToString());
            return fields.ToArray();
        }
    }
}