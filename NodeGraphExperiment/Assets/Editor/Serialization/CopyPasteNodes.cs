using System;
using System.Collections.Generic;
using System.Linq;
using Editor.Drawing;
using Editor.Drawing.Nodes;
using Editor.Factories;
using Editor.Factories.NodeListeners;
using Runtime.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Editor.Serialization
{
    [Serializable]
    public class CopyPasteNodes
    {
        public List<GraphElement> ElementsToCopy = new List<GraphElement>();
        
        private readonly DialogueNodeFactory _factory;
        private readonly NodesProvider _provider;

        public CopyPasteNodes(DialogueNodeFactory factory, NodesProvider provider)
        {
            _factory = factory;
            _provider = provider;
        }


        public void Add(GraphElement element) =>
            ElementsToCopy.Add(element);

        public void Clear() =>
            ElementsToCopy.Clear();

        public void Paste(DialogueGraphView graphView)
        {
            var mapping = new Dictionary<string, string>();

            foreach (var element in ElementsToCopy)
            {
                if (element is DialogueNodeView nodeView)
                {
                    var model = Copy(nodeView.Model);
                    var node = _factory.CreateWithoutUndo(model);
                    mapping.Add(nodeView.Model.Guid, model.Guid);
                    graphView.AddToSelection(node);
                }
            }

            foreach (var element in ElementsToCopy)
            {
                if (element is Edge edge)
                {
                    if (edge.output == null || edge.input == null)
                        continue;

                    var parentNode = edge.output.node as DialogueNodeView;
                    var childNode = edge.input.node as DialogueNodeView;
                    
                    if (!mapping.ContainsKey(parentNode.Model.Guid) 
                        || !mapping.ContainsKey(childNode.Model.Guid))
                        continue;

                    var parentGuid = mapping[parentNode.Model.Guid];
                    var childGuid = mapping[childNode.Model.Guid];

                    var parent = _provider.GetById(parentGuid);
                    var child = _provider.GetById(childGuid);
                    var newEdge = Connect(parent.outputContainer[0] as Port, child.inputContainer[0] as Port);
                    graphView.AddElement(newEdge);
                    graphView.AddToSelection(newEdge);
                }
            }
        }

        private DialogueNode Copy(DialogueNode original) =>
            new DialogueNode()
            {
                Guid = Guid.NewGuid().ToString(),
                PathToImage = original.PathToImage,
                PersonId = original.PersonId,
                PhraseId = original.PhraseId,
                Position = new Rect(original.Position.position + new Vector2(25, 25), original.Position.size)
            };

        private static Edge Connect(Port output, Port input)
        {
            var edge = new Edge() {output = output, input = input};
            input.Connect(edge);
            output.Connect(edge);
            return edge;
        }
    }
}