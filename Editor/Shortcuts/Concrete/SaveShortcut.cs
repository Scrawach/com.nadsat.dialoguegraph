using Nadsat.DialogueGraph.Editor.Windows;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Shortcuts.Concrete
{
    public class SaveShortcut : ICustomShortcut
    {
        private readonly DialogueGraphRoot _root;

        public SaveShortcut(DialogueGraphRoot root) =>
            _root = root;

        public bool IsHandle(KeyDownEvent keyDown) =>
            keyDown.keyCode == KeyCode.S
            && keyDown.modifiers == EventModifiers.Control;

        public void Handle(KeyDownEvent keyDown) =>
            _root.Save();
    }
}