using System;
using Runtime.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Drawing.Nodes
{
    public abstract class BaseNodeView<TModel> : Node, IMovableNode, ISelectableNode, IModelHandle, IRootable
        where TModel : BaseDialogueNode
    {
        private const string UssEntryNode = "entry-node";

        private readonly VisualElement _nodeBorder;
        
        protected BaseNodeView(string uxml) : base(uxml) =>
            _nodeBorder = this.Q<VisualElement>("node-border");

        protected BaseNodeView() { }
        
        public TModel Model { get; private set; }

        BaseDialogueNode IModelHandle.Model => Model;
        
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
        
        public void MarkAsRoot(bool isRoot)
        {
            if (isRoot)
                _nodeBorder.AddToClassList(UssEntryNode);
            else
                _nodeBorder.RemoveFromClassList(UssEntryNode);
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

        public void AddOutput(string portName = "") =>
            AddOutput(portName, string.Empty);

        public void AddOutput(string portName = "", string portId = "")
        {
            var output = CreatePort(portName, Direction.Output);
            if (!string.IsNullOrWhiteSpace(portId))
                output.viewDataKey = portId;
            outputContainer.Add(output);
            RefreshPorts();
        }

        private Port CreatePort(string portName, Direction direction)
        {
            var port = InstantiatePort(Orientation.Horizontal, direction, Port.Capacity.Multi, typeof(float));
            port.portName = portName;
            return ConfiguredPortStyle(port);
        }

        private Port ConfiguredPortStyle(Port port)
        {
            port.style.height = new StyleLength(StyleKeyword.Auto);
            port.style.minHeight = new StyleLength(new Length(24, LengthUnit.Pixel));
            port.Q<Label>().style.height = new StyleLength(StyleKeyword.Auto);
            port.Q<Label>().style.whiteSpace = new StyleEnum<WhiteSpace>(WhiteSpace.Normal);
            return port;
        }

        protected abstract void OnModelChanged();

        public Rect GetPreviousPosition() =>
            Model.Position;

        public void SavePosition(Rect position) =>
            Model.SetPosition(position);
    }
}