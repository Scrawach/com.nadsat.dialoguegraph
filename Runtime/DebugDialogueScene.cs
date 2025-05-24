using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Nadsat.DialogueGraph.Runtime
{
    public class DebugDialogueScene : MonoBehaviour
    {
        public DialogueGraph Graph;
        public List<TextAsset> Localization;

        public UnityEvent<DialogueGraph, List<TextAsset>> Started;

        private void Start() => 
            Started?.Invoke(Graph, Localization);
    }
}