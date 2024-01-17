using Editor.Drawing.Nodes;
using Editor.Windows.Variables;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Factories
{
    public class VariableNodeFactory
    {
        private readonly GraphView _canvas;
        private readonly VariablesProvider _variables;

        public VariableNodeFactory(GraphView canvas, VariablesProvider variables)
        {
            _canvas = canvas;
            _variables = variables;
        }
        
        public VariableNodeView Create(Vector2 position, string value)
        {
            var node = new VariableNodeView(_variables);
            node.SetVariable(value);
            var localPosition = _canvas.contentViewContainer.WorldToLocal(position);
            var input = node.InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
            input.portName = "";
            var output = node.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
            output.portName = "";
            node.inputContainer.Add(input);
            node.outputContainer.Add(output);
            node.RefreshPorts();
            node.RefreshExpandedState();
            
            node.SetPosition(new Rect(localPosition, Vector2.zero));
            _canvas.AddElement(node);
            return node;
        }
    }
}