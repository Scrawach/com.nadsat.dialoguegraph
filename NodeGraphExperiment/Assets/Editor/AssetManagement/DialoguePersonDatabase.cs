using System.Collections.Generic;
using System.Linq;
using Editor.Data;
using UnityEngine;

namespace Editor.AssetManagement
{
    [CreateAssetMenu(fileName = "Dialogue Person Database", menuName = "Dialogue Graph/Person Database", order = 0)]
    public class DialoguePersonDatabase : ScriptableObject
    {
        public List<PersonData> Persons;

        public PersonData FindByName(string personName) =>
            Persons.FirstOrDefault(person => person.Name == personName);
    }
}