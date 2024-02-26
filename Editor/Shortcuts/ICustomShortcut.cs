using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Shortcuts
{
    public interface ICustomShortcut
    {
        bool IsHandle(KeyDownEvent keyDown);
        void Handle(KeyDownEvent keyDown);
    }
}