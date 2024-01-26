using UnityEngine;

namespace Runtime
{
    [CreateAssetMenu(fileName = "DialogueGraphData", menuName = "Dialogue Graph/Create", order = 0)]
    public class DialogueGraphContainer : ScriptableObject
    {
        public DialogueGraph Graph;
    }
}