using System;
using System.IO;
using System.Linq;
using Nadsat.DialogueGraph.Runtime;
using UnityEditor;
using UnityEngine;

namespace Nadsat.DialogueGraph.Editor.AssetManagement
{
    public class DialogueDatabase
    {
        private readonly PersonData _none = new() {Color = Color.gray, Icon = null, Name = "none"};
        private DialogueGraphDatabase _database;

        public void Initialize()
        {
            var path = Path.Combine("Assets", "Resources", "Dialogues", "Dialogue Database.asset");
            _database = GetOrCreateDatabase(path);
        }

        public string[] All() => 
            _database.Persons.Count < 1 
                ? Array.Empty<string>() 
                : _database.Persons.Select(x => x.Name).ToArray();

        public string[] AllEmotions(string personId) => 
            Get(personId).Emotions.Select(emotion => emotion.Name).ToArray();

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
            var emptyDatabase = ScriptableObject.CreateInstance<DialogueGraphDatabase>();
            
            var directoryPath = Path.GetDirectoryName(path)!;
            Directory.CreateDirectory(directoryPath);
            
            AssetDatabase.CreateAsset(emptyDatabase, path);
            return emptyDatabase;
        }
    }
}