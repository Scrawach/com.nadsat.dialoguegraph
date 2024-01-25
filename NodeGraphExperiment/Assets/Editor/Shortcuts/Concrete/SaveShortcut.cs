using Editor.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Shortcuts.Concrete
{
    public class SaveShortcut : ICustomShortcut
    {
        private readonly DialogueGraphView _view;

        public SaveShortcut(DialogueGraphView view) =>
            _view = view;
        
        public bool IsHandle(KeyDownEvent keyDown) =>
            keyDown.keyCode == KeyCode.S
            && keyDown.modifiers == EventModifiers.Control;

        public void Handle(KeyDownEvent keyDown) =>
            _view.Save();
    }
}