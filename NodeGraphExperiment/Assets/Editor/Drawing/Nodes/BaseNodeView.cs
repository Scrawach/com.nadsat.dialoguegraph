using Runtime.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Editor.Drawing.Nodes
{
    public abstract class BaseNodeView<TModel> : Node, IMovableNode where TModel : BaseDialogueNode
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

        public Rect GetPreviousPosition() =>
            Model.Position;

        public void SavePosition(Rect position) =>
            Model.SetPosition(position);
    }
}