using System.Collections.Generic;
using UnityEngine;

namespace Nadsat.DialogueGraph.Runtime
{
    [CreateAssetMenu(fileName = "Dialogue Database", menuName = "Dialogue Graph/Database", order = 0)]
    public class DialogueGraphDatabase : ScriptableObject
    {
        public List<PersonData> Persons;
        public List<string> GlobalVariableNames;
    }
}