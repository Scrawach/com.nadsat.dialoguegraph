using Nadsat.DialogueGraph.Editor.Undo;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Shortcuts.Concrete
{
    public class UndoShortcut : ICustomShortcut
    {
        private readonly IUndoHistory _undoHistory;

        public UndoShortcut(IUndoHistory undoHistory) =>
            _undoHistory = undoHistory;

        public bool IsHandle(KeyDownEvent keyDown) =>
            keyDown.keyCode == KeyCode.Z
            && keyDown.modifiers == EventModifiers.Control;

        public void Handle(KeyDownEvent keyDown) =>
            _undoHistory.Undo();
    }
}