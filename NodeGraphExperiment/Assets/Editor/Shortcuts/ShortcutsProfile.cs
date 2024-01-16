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

        public ShortcutsProfile(SearchWindowProvider searchWindow, DialogueGraphView graphView)
        {
            _searchWindowProvider = searchWindow;
            _graphView = graphView;
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