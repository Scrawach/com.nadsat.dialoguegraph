using System.Linq;
using Editor.Data;
using UnityEngine;

namespace Editor.AssetManagement
{
    public class PersonRepository
    {
        private DialoguePersonDatabase _database;

        public void Initialize() =>
            _database = Resources.Load<DialoguePersonDatabase>("Dialogue Person Database");

        public string[] All() =>
            _database.Persons.Select(x => x.Name).ToArray();

        public PersonData Get(string key) =>
            _database.FindByName(key);
    }
}