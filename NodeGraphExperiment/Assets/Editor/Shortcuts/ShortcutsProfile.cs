using Editor.Undo;
using Editor.Windows.Search;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Shortcuts
{
    public class ShortcutsProfile
    {
        private readonly SearchWindowProvider _searchWindowProvider;
        private readonly DialogueGraphView _graphView;
        private readonly IUndoHistory _undoHistory;

        public ShortcutsProfile(SearchWindowProvider searchWindow, DialogueGraphView graphView, IUndoHistory undoHistory)
        {
            _searchWindowProvider = searchWindow;
            _graphView = graphView;
            _undoHistory = undoHistory;
        }

        public void Handle(KeyDownEvent keyDown)
        {
            if (IsFind(keyDown))
            {
                _searchWindowProvider.FindNodes(keyDown.originalMousePosition, view =>
                {
                    _graphView.Find(view);
                });
            }
            else if (IsUndo(keyDown))
            {
                _undoHistory.Undo();
            }
            else if (IsRedo(keyDown))
            {
                _undoHistory.Redo();
            }
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