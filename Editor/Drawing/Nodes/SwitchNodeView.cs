using System;
using System.Collections.Generic;
using System.Linq;
using Nadsat.DialogueGraph.Editor.Drawing.Nodes.Abstract;
using Nadsat.DialogueGraph.Runtime.Nodes;
using UnityEditor.Experimental.GraphView;

namespace Nadsat.DialogueGraph.Editor.Drawing.Nodes
{
    public class SwitchNodeView : BaseNodeView<SwitchNode>, IRemovablePorts
    {
        private const string UxmlPath = "UXML/SwitchNodeView";

        public SwitchNodeView() : base(UxmlPath) { }

        public event Action<IEnumerable<Port>> PortRemoved;

        protected override void OnModelChanged()
        {
            var ports = outputContainer.Children().Cast<Port>().ToArray();
            PortRemoved?.Invoke(GetUnusedPorts(Model, ports));
            CreateMissingOutputPorts(Model, ports);
        }

        private static IEnumerable<Port> GetUnusedPorts(SwitchNode model, IEnumerable<Port> ports) =>
            ports.Where(port => model.Branches.FirstOrDefault(b => b.Condition == port.viewDataKey) == null);

        private void CreateMissingOutputPorts(SwitchNode model, Port[] ports)
        {
            foreach (var branch in model.Branches)
            {
                if (ports.FirstOrDefault(port => port.viewDataKey == branch.Condition) != null)
                    continue;

                AddOutput(branch.Condition, branch.Condition);
            }
        }
    }
}