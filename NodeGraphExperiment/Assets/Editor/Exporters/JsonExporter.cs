using System.Collections.Generic;
using System.IO;
using System.Linq;
using Editor.Drawing.Nodes;
using Runtime;
using Runtime.Nodes;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Editor.Exporters
{
    public class JsonExporter
    {
        public void Export(string graphName, GraphView view)
        {
            var graph = new NodeGraph();
            graph.Name = graphName;
            graph.Nodes = AllModelHandlersFrom(view).Select(node => node.Model).ToList();
            graph.Links = GetLinksFrom(view).ToList();

            var settings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Objects};
            var json = JsonConvert.SerializeObject(graph, settings);
            var pathToAsset = $"Resources/Dialogues/{graphName}/{graphName}.json";
            var absolutePathToAsset = Path.Combine(Application.dataPath, pathToAsset);
            File.WriteAllText(absolutePathToAsset, json);
            AssetDatabase.SaveAssets();
        }

        private static IEnumerable<IModelHandle> AllModelHandlersFrom(GraphView view) =>
            view.nodes.Select(node => node as IModelHandle);

        private static IEnumerable<NodeLinks> GetLinksFrom(GraphView view)
        {
            foreach (var edge in view.edges.Where(e => e.input.node != null))
            {
                var parentNode = (dynamic) edge.output.node;
                var childNode = (dynamic) edge.input.node;

                yield return new NodeLinks()
                {
                    FromGuid = parentNode.Model.Guid,
                    FromPortId = edge.output.viewDataKey,
                    ToGuid = childNode.Model.Guid,
                    ToPortId = edge.input.viewDataKey
                };
            }
        }
    }
    
    public class NodeGraph
    {
        public string Name;
        public List<BaseDialogueNode> Nodes;
        public List<NodeLinks> Links;
    }
}