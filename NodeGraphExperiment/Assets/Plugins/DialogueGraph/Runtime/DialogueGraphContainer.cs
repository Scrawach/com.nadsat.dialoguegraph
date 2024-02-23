using UnityEngine;

namespace Nadsat.DialogueGraph.Runtime
{
    [CreateAssetMenu(fileName = "DialogueGraphData", menuName = "Dialogue Graph/Create", order = 0)]
    public class DialogueGraphContainer : ScriptableObject
    {
        public DialogueGraph Graph;
    }
}