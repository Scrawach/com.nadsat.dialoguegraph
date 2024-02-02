using System;
using System.Collections.Generic;
using System.Linq;
using Editor.AssetManagement;
using Runtime.Nodes;
using UnityEditor.Experimental.GraphView;

namespace Editor.Drawing.Nodes
{
    public class ChoicesNodeView : BaseNodeView<ChoicesNode>
    {
        private const string UxmlPath = "Assets/Editor/Resources/UXML/ChoicesNodeView.uxml";
        
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

        private static IEnumerable<Port> GetUnusedPorts(ChoicesNode model, IEnumerable<Port> ports) =>
            ports.Where(port => !model.Choices.Contains(port.viewDataKey));

        private void CreateMissingOutputPorts(ChoicesNode model, Port[] ports)
        {
            foreach (var choice in model.Choices)
            {
                if (ports.FirstOrDefault(port => port.viewDataKey == choice) != null)
                    continue;
                
                AddOutput(_choices.Get(choice), choice);
            }
        }
    }
}