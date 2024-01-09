using System.Collections.Generic;
using UnityEngine;

namespace Editor
{
    [CreateAssetMenu(fileName = "Dialogue Person Database", menuName = "Dialogue Graph/Person Database", order = 0)]
    public class DialoguePersonDatabase : ScriptableObject
    {
        public List<DialoguePersonData> Persons;
    }
}