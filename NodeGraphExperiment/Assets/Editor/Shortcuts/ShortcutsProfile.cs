using System.Linq;
using UnityEngine.UIElements;

namespace Editor.Shortcuts
{
    public class ShortcutsProfile
    {
        private readonly ICustomShortcut[] _shortcuts;

        public ShortcutsProfile(params ICustomShortcut[] shortcuts) =>
            _shortcuts = shortcuts;

        public void Handle(KeyDownEvent keyDown)
        {
            foreach (var shortcut in _shortcuts.Where(s => s.IsHandle(keyDown)))
            {
                shortcut.Handle(keyDown);
                keyDown.StopPropagation();
            }
        }
    }
}