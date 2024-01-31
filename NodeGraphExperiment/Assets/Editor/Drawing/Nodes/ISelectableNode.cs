using System;
using Runtime.Nodes;
using UnityEditor.Experimental.GraphView;

namespace Editor.Drawing.Nodes
{
    public interface ISelectableNode
    {
        event Action<Node> Selected;
        event Action<Node> UnSelected;
    }

    public interface IModelHandle
    {
        BaseDialogueNode Model { get; }
    }
}