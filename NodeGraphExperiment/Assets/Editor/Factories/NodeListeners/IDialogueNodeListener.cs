using UnityEditor.Experimental.GraphView;

namespace Editor.Factories.NodeListeners
{
    public interface IDialogueNodeListener
    {
        void Register(Node node);
        void Unregister(Node node);
    }
}