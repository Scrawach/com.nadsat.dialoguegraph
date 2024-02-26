using System;
using UnityEditor.Experimental.GraphView;

namespace Nadsat.DialogueGraph.Editor.Drawing.Nodes
{
    public interface ISelectableNode
    {
        event Action<Node> Selected;
        event Action<Node> UnSelected;
    }
}