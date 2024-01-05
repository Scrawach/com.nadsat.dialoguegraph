using System;
using System.IO;
using UnityEditor.Experimental.GraphView;

namespace Editor
{
    public class DialogueNodeView : Node
    {
        public string Guid;

        public DialogueNodeView() : base(Path.Combine("Assets", "Editor", "DialogueNodeView.uxml"))
        {
        }
        
        public event Action<DialogueNodeView> OnNodeSelected;

        public override void OnSelected() =>
            OnNodeSelected?.Invoke(this);
    }
}