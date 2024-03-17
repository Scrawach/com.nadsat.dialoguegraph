using System;
using Nadsat.DialogueGraph.Runtime.Data;
using UnityEngine;

namespace Nadsat.DialogueGraph.Runtime.Nodes
{
    public class BaseDialogueNode
    {
        public string Guid;
        public Vector2Data Position;

        public event Action Changed;

        public void SetPosition(Rect position)
        {
            Position = position;
            NotifyChanged();
        }

        public void NotifyChanged() =>
            Changed?.Invoke();
    }
}