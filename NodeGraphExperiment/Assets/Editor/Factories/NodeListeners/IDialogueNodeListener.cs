using Editor.Drawing.Nodes;

namespace Editor.Factories.NodeListeners
{
    public interface IDialogueNodeListener
    {
        void Register(DialogueNodeView node);
        void Unregister(DialogueNodeView node);
    }
}