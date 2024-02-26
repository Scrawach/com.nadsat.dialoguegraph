using Nadsat.DialogueGraph.Editor.Drawing;
using Nadsat.DialogueGraph.Editor.Windows.Search;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Shortcuts.Concrete
{
    public class FindShortcut : ICustomShortcut
    {
        private readonly DialogueGraphView _graphView;
        private readonly SearchWindowProvider _searchWindowProvider;

        public FindShortcut(SearchWindowProvider searchWindowProvider, DialogueGraphView graphView)
        {
            _searchWindowProvider = searchWindowProvider;
            _graphView = graphView;
        }

        public bool IsHandle(KeyDownEvent keyDown) =>
            keyDown.keyCode == KeyCode.F
            && keyDown.modifiers == EventModifiers.None;

        public void Handle(KeyDownEvent keyDown) =>
            _searchWindowProvider.FindNodes(keyDown.originalMousePosition, view => { _graphView.Find(view); });
    }
}