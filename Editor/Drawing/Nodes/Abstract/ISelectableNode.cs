using System;
using UnityEditor.Experimental.GraphView;

namespace Nadsat.DialogueGraph.Editor.Drawing.Nodes.Abstract
{
    public interface ISelectableNode
    {
        event Action<Node> Selected;
        event Action<Node> UnSelected;
    }
}