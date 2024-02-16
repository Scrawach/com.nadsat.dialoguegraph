using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;

namespace Editor.Undo.Commands
{
    public class RemoveEdges : IUndoCommand
    {
        private readonly GraphView _canvas;
        private readonly Dictionary<Edge, (Port input, Port output)> _edgesWithPorts;

        public RemoveEdges(GraphView canvas, IEnumerable<Edge> edgesToRemove)
        {
            _canvas = canvas;
            _edgesWithPorts = edgesToRemove.ToDictionary(x => x, x => (x.input, x.output));
        }

        public void Undo()
        {
            foreach (var edgesWithPort in _edgesWithPorts)
            {
                var edge = edgesWithPort.Key;
                var (input, output) = edgesWithPort.Value;
                edge.output = output;
                edge.input = input;
                input?.Connect(edge);
                output?.Connect(edge);
                edge.UpdateEdgeControl();
                _canvas.AddElement(edge);
            }
        }

        public void Redo()
        {
            foreach (var edgesWithPort in _edgesWithPorts)
            {
                var edge = edgesWithPort.Key;
                edge.output?.Disconnect(edge);
                edge.input?.Disconnect(edge);
                edge.UpdateEdgeControl();
                _canvas.RemoveElement(edge);
            }
        }
    }
}