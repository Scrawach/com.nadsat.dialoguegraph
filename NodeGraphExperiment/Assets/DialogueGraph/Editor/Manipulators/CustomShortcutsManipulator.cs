using Editor.Shortcuts;
using UnityEngine.UIElements;

namespace Editor.Manipulators
{
    public class CustomShortcutsManipulator : Manipulator
    {
        private readonly ShortcutsProfile _shortcuts;

        public CustomShortcutsManipulator(ShortcutsProfile shortcuts) =>
            _shortcuts = shortcuts;

        protected override void RegisterCallbacksOnTarget() =>
            target.RegisterCallback<KeyDownEvent>(_shortcuts.Handle);

        protected override void UnregisterCallbacksFromTarget() =>
            target.UnregisterCallback<KeyDownEvent>(_shortcuts.Handle);
    }
}