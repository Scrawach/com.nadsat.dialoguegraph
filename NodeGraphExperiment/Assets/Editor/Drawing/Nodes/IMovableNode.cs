using UnityEngine;

namespace Editor.Drawing.Nodes
{
    public interface IMovableNode
    {
        Rect GetPosition();
        Rect GetPreviousPosition();
        void SavePosition(Rect position);
    }
}