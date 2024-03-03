using System;
using UnityEngine;

namespace Nadsat.DialogueGraph.Runtime.Nodes
{
    [Serializable]
    public class NoteNode : BaseDialogueNode
    {
        public string Title;
        public string Description;
        public Vector2 Size;
    }
}