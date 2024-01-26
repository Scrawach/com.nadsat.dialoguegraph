using Runtime.Nodes;
using UnityEditor.Experimental.GraphView;

namespace Editor.Drawing.Nodes
{
    public abstract class BaseNodeView<TModel> : Node where TModel : BaseDialogueNode
    {
        protected BaseNodeView(string uxml) : base(uxml) { }

        protected BaseNodeView() { }
        
        public TModel Model { get; private set; }

        public void Bind(TModel model)
        {
            Model = model;
            Model.Changed += OnModelChanged;
            OnModelChanged();
        }

        public void Unbind()
        {
            Model.Changed += OnModelChanged;
            Model = null;
        }

        protected abstract void OnModelChanged();
    }
}