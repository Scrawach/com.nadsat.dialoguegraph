using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Undo
{
    public class UndoHandler
    {
        public void PerformUndo()
        {
            Debug.Log($"Perform UNDO");
        }

        public void PerformRedo()
        {
            Debug.Log($"Perform REDO");
        }

        public void Handle(KeyDownEvent keyDown)
        {
            if (IsUndo(keyDown))
                PerformUndo();
            else if (IsRedo(keyDown))
                PerformRedo();
        }

        private bool IsRedo(KeyDownEvent keyDown) =>
            keyDown.keyCode == KeyCode.Z 
            && keyDown.modifiers == EventModifiers.Control;

        private bool IsUndo(KeyDownEvent keyDown) =>
            keyDown.keyCode == KeyCode.Z 
            && keyDown.modifiers.HasFlag(EventModifiers.Shift)
            && keyDown.modifiers.HasFlag(EventModifiers.Control);
    }
}