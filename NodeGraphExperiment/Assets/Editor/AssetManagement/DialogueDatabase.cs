using System.IO;
using System.Linq;
using Runtime;
using UnityEditor;
using UnityEngine;

namespace Editor.AssetManagement
{
    public class DialogueDatabase
    {
        private DialogueGraphDatabase _database;

        private readonly PersonData _none = new() {Color = Color.gray, Icon = null, Name = "none"};

        public void Initialize()
        {
            var path = Path.Combine("Assets", "Resources", "Dialogues", "Dialogue Database.asset");
            _database = GetOrCreateDatabase(path);
        }
        
        public string[] All() =>
            _database.Persons.Select(x => x.Name).ToArray();

        public PersonData Get(string key) =>
            _database.Persons.FirstOrDefault(p => p.Name == key) ?? _none;

        private DialogueGraphDatabase GetOrCreateDatabase(string path)
        {
            var database = AssetDatabase.LoadAssetAtPath<DialogueGraphDatabase>(path);

            if (database == null)
                database = CreateDatabase(path);

            return database;
        }

        private DialogueGraphDatabase CreateDatabase(string path)
        {
            Debug.Log($"TRYING CREATE!");
            return null;
        }
    }
}