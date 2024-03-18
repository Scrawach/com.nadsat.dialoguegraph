using System;
using System.Collections.Generic;
using System.Linq;
using Nadsat.DialogueGraph.Editor.AssetManagement;
using Nadsat.DialogueGraph.Editor.Drawing.Nodes.Abstract;
using Nadsat.DialogueGraph.Runtime.Data;
using Nadsat.DialogueGraph.Runtime.Nodes;
using UnityEditor.Experimental.GraphView;

namespace Nadsat.DialogueGraph.Editor.Drawing.Nodes
{
    public class ChoicesNodeView : BaseNodeView<ChoicesNode>, IRemovablePorts
    {
        private const string UxmlPath = "UXML/ChoicesNodeView";

        private readonly ChoicesRepository _choices;

        public ChoicesNodeView(ChoicesRepository choices) : base(UxmlPath) =>
            _choices = choices;

        public event Action<IEnumerable<Port>> PortRemoved;

        protected override void OnModelChanged()
        {
            var ports = outputContainer.Children().Cast<Port>().ToArray();
            PortRemoved?.Invoke(GetUnusedPorts(Model, ports));
            CreateMissingOutputPorts(Model, ports);
        }

        private static IEnumerable<Port> GetUnusedPorts(ChoicesNode model, IEnumerable<Port> ports)
        {
            var choiceIds = model.Choices.Select(x => x.ChoiceId);
            return ports.Where(p => !choiceIds.Contains(p.viewDataKey));
        }

        private void CreateMissingOutputPorts(ChoicesNode model, Port[] ports)
        {
            foreach (var choice in model.Choices)
            {
                var port = ports.FirstOrDefault(p => p.viewDataKey == choice.ChoiceId);

                if (port != null)
                {
                    port.portName = _choices.Get(choice.ChoiceId);
                    continue;
                }

                AddOutput(_choices.Get(choice.ChoiceId), choice.ChoiceId);
            }
        }
    }
}