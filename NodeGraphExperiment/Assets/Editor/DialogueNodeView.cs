using System;
using UnityEditor.Experimental.GraphView;

namespace Editor
{
    public class DialogueNodeView : Node
    {
        public string Guid;
        
        public event Action<DialogueNodeView> OnNodeSelected;

        public override void OnSelected() =>
            OnNodeSelected?.Invoke(this);
    }
}