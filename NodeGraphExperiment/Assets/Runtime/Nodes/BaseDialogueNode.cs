using System;
using UnityEngine;

namespace Runtime.Nodes
{
    public class BaseDialogueNode
    {
        public string Guid;
        public Rect Position;

        public event Action Changed;
        
        public void SetPosition(Rect position)
        {
            Position = position;
            NotifyChanged();
        }

        protected void NotifyChanged() =>
            Changed?.Invoke();
    }
}