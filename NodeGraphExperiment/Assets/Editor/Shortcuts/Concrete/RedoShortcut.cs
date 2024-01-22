using Editor.Undo;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Shortcuts.Concrete
{
    public class RedoShortcut : ICustomShortcut
    {
        private readonly IUndoHistory _undoHistory;

        public RedoShortcut(IUndoHistory undoHistory) =>
            _undoHistory = undoHistory;
        
        public bool IsHandle(KeyDownEvent keyDown) =>
            keyDown.keyCode == KeyCode.Z 
            && keyDown.modifiers.HasFlag(EventModifiers.Shift)
            && keyDown.modifiers.HasFlag(EventModifiers.Control);

        public void Handle(KeyDownEvent keyDown) =>
            _undoHistory.Redo();
    }
}