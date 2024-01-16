using Editor.Undo;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Shortcuts
{
    public class ShortcutsProfile
    {
        private readonly IUndoHistory _undoHistory;

        public ShortcutsProfile(IUndoHistory undoHistory) =>
            _undoHistory = undoHistory;
        
        public void Handle(KeyDownEvent keyDown)
        {
            if (IsFind(keyDown))
                Debug.Log($"Open Find Window");
            else if (IsRedo(keyDown))
                _undoHistory.Redo();
            else if (IsUndo(keyDown)) 
                _undoHistory.Undo();
        }

        private static bool IsFind(IKeyboardEvent keyDown) =>
            keyDown.keyCode == KeyCode.F
            && keyDown.modifiers == EventModifiers.None;
        
        private static bool IsUndo(IKeyboardEvent keyDown) =>
            keyDown.keyCode == KeyCode.Z 
            && keyDown.modifiers == EventModifiers.Control;

        private static bool IsRedo(IKeyboardEvent keyDown) =>
            keyDown.keyCode == KeyCode.Z 
            && keyDown.modifiers.HasFlag(EventModifiers.Shift)
            && keyDown.modifiers.HasFlag(EventModifiers.Control);
    }
}