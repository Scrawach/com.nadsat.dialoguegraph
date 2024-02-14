using System;
using System.Collections.Generic;
using System.Linq;
using Editor.ContextualMenu;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Shortcuts.Concrete
{
    public class TemplateHotkeyShortcuts : ICustomShortcut
    {
        private readonly List<(KeyCode keys, Action<Vector2> action)> _hotkeys;

        public TemplateHotkeyShortcuts(TemplateDialogueFactory templateFactory) =>
            _hotkeys = new List<(KeyCode keys, Action<Vector2> action)>()
            {
                (KeyCode.Alpha1, (position) => templateFactory.Create("Elena", position)),
                (KeyCode.Alpha2, (position) => templateFactory.Create("Mark", position))
            };

        public bool IsHandle(KeyDownEvent keyDown) =>
            _hotkeys.Any(hotkey => hotkey.keys == keyDown.keyCode);

        public void Handle(KeyDownEvent keyDown)
        {
            foreach (var hotkey in _hotkeys.Where(hotkey => hotkey.keys == keyDown.keyCode)) 
                hotkey.action(keyDown.originalMousePosition);
        }
    }
}