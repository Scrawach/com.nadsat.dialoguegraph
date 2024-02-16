using System.Collections.Generic;
using System.Linq;
using Editor.Drawing.Nodes;
using Runtime;
using Runtime.Nodes;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor.Experimental.GraphView;

namespace Editor.Serialization
{
    public class CopyPaste
    {
        private readonly JsonSerializerSettings _settings = new()
        {
            TypeNameHandling = TypeNameHandling.Objects
        };

        public string ToJson(IEnumerable<GraphElement> elements)
        {
            var arrayElements = elements.ToArray();
            var graphData = new CopiedGraphData
            {
                Nodes = AllModelHandlersFrom(arrayElements).Select(m => m.Model).ToArray(),
                Links = GetLinksFrom(EdgesFrom(arrayElements)).ToArray()
            };
            return JsonConvert.SerializeObject(graphData, _settings);
        }

        public CopiedGraphData FromJson(string json) =>
            JsonConvert.DeserializeObject<CopiedGraphData>(json, _settings);

        private static IEnumerable<IModelHandle> AllModelHandlersFrom(IEnumerable<GraphElement> elements) =>
            elements.Select(node => node as IModelHandle).Where(n => n != null);

        private static IEnumerable<Edge> EdgesFrom(IEnumerable<GraphElement> elements) =>
            elements.Select(element => element as Edge).Where(edge => edge != null);

        private static IEnumerable<NodeLinks> GetLinksFrom(IEnumerable<Edge> edges)
        {
            foreach (var edge in edges.Where(e => e.input.node != null))
            {
                var parentNode = (dynamic) edge.output.node;
                var childNode = (dynamic) edge.input.node;

                yield return new NodeLinks
                {
                    FromGuid = parentNode.Model.Guid,
                    FromPortId = edge.output.viewDataKey,
                    ToGuid = childNode.Model.Guid,
                    ToPortId = edge.input.viewDataKey
                };
            }
        }

        public class CopiedGraphData
        {
            public NodeLinks[] Links;
            public BaseDialogueNode[] Nodes;
        }
    }
}