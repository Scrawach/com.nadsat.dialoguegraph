using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Shortcuts.Concrete
{
    public class SaveShortcut : ICustomShortcut
    {
        public bool IsHandle(KeyDownEvent keyDown) =>
            keyDown.keyCode == KeyCode.S
            && keyDown.modifiers == EventModifiers.Control;

        public void Handle(KeyDownEvent keyDown) =>
            Debug.Log("Saved!");
    }
}