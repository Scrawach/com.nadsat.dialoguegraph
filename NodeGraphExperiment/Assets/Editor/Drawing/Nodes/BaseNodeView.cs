using System;
using Runtime.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Editor.Drawing.Nodes
{
    public abstract class BaseNodeView<TModel> : Node, IMovableNode, ISelectableNode 
        where TModel : BaseDialogueNode
    {
        protected BaseNodeView(string uxml) : base(uxml) { }

        protected BaseNodeView() { }
        
        public TModel Model { get; private set; }
        
        public event Action<Node> Selected;
        public event Action<Node> UnSelected; 

        public override void OnSelected()
        {
            base.OnSelected();
            Selected?.Invoke(this);
        }

        public override void OnUnselected()
        {
            base.OnUnselected();
            UnSelected?.Invoke(this);
        }

        public void Bind(TModel model)
        {
            Model = model;
            Model.Changed += OnModelChanged;
            OnModelChanged();
        }

        public void Unbind()
        {
            Model.Changed -= OnModelChanged;
            Model = null;
        }

        public void AddInputAndOutputPorts(string inputPortName = "", string outputPortName = "")
        {
            AddInput(inputPortName);
            AddOutput(outputPortName);
        }

        public void AddInput(string portName = "")
        {
            var input = CreatePort(portName, Direction.Input);
            inputContainer.Add(input);
            RefreshPorts();
        }

        public void AddOutput(string portName = "")
        {
            var output = CreatePort(portName, Direction.Output);
            outputContainer.Add(output);
            RefreshPorts();
        }

        private Port CreatePort(string portName, Direction direction)
        {
            var port = InstantiatePort(Orientation.Horizontal, direction, Port.Capacity.Multi, typeof(float));
            port.portName = portName;
            return port;
        }

        protected abstract void OnModelChanged();

        public Rect GetPreviousPosition() =>
            Model.Position;

        public void SavePosition(Rect position) =>
            Model.SetPosition(position);
    }
}