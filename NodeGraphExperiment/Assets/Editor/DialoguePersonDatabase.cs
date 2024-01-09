using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Editor
{
    [CreateAssetMenu(fileName = "Dialogue Person Database", menuName = "Dialogue Graph/Person Database", order = 0)]
    public class DialoguePersonDatabase : ScriptableObject
    {
        public List<DialoguePersonData> Persons;

        public DialoguePersonData FindByName(string personName) =>
            Persons.First(person => person.Name == personName);
    }
}