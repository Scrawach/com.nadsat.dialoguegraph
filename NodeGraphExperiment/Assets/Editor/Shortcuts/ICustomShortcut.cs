using UnityEngine.UIElements;

namespace Editor.Shortcuts
{
    public interface ICustomShortcut
    {
        bool IsHandle(KeyDownEvent keyDown);
        void Handle(KeyDownEvent keyDown);
    }
}