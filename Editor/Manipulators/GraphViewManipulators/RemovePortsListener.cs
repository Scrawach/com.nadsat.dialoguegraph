using System.Collections.Generic;
using System.Linq;
using Nadsat.DialogueGraph.Editor.Drawing.Nodes;
using Nadsat.DialogueGraph.Editor.Drawing.Nodes.Abstract;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Manipulators.GraphViewManipulators
{
    public class RemovePortsListener : Manipulator
    {
        private readonly GraphView _graphView;

        public RemovePortsListener(GraphView graphView) =>
            _graphView = graphView;

        protected override void RegisterCallbacksOnTarget()
        {
            var removablePorts = (IRemovablePorts) target;
            removablePorts.PortRemoved += OnPortsRemoved;
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            var removablePorts = (IRemovablePorts) target;
            removablePorts.PortRemoved -= OnPortsRemoved;
        }

        private void OnPortsRemoved(IEnumerable<Port> ports)
        {
            foreach (var port in ports)
            {
                var connections = port.connections.ToArray();
                _graphView.RemoveElement(port);
                foreach (var edge in connections)
                {
                    edge.input.Disconnect(edge);
                    edge.output.Disconnect(edge);
                    _graphView.RemoveElement(edge);
                }
            }
        }
    }
}