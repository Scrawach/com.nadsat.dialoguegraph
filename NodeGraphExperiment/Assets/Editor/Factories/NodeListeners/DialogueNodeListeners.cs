using UnityEditor.Experimental.GraphView;

namespace Editor.Factories.NodeListeners
{
    public class DialogueNodeListeners : IDialogueNodeListener
    {
        private readonly IDialogueNodeListener[] _listeners;

        public DialogueNodeListeners(params IDialogueNodeListener[] listeners) =>
            _listeners = listeners;
        
        public void Register(Node node)
        {
            foreach (var listener in _listeners) 
                listener.Register(node);
        }

        public void Unregister(Node node)
        {
            foreach (var listener in _listeners) 
                listener.Unregister(node);
        }
    }
}