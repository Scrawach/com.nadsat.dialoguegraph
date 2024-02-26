using UnityEngine;

namespace Nadsat.DialogueGraph.Editor.Drawing.Nodes
{
    public interface IMovableNode
    {
        Rect GetPosition();
        Rect GetPreviousPosition();
        void SavePosition(Rect position);
    }
}