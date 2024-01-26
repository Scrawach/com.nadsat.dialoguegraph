using System;
using System.Collections.Generic;
using System.Linq;
using Editor.Drawing;
using Editor.Drawing.Nodes;
using Editor.Factories;
using Editor.Factories.NodeListeners;
using Editor.Undo;
using Editor.Undo.Commands;
using Runtime.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Editor.Serialization
{
    [Serializable]
    public class CopyPasteNodes
    {
        public List<GraphElement> ElementsToCopy = new List<GraphElement>();
        
        private readonly INodeViewFactory _factory;
        private readonly NodesProvider _provider;
        private readonly IUndoRegister _undoRegister;

        public CopyPasteNodes(INodeViewFactory factory, NodesProvider provider, IUndoRegister undoRegister)
        {
            _factory = factory;
            _provider = provider;
            _undoRegister = undoRegister;
        }


        public void Add(GraphElement element) =>
            ElementsToCopy.Add(element);

        public void Clear() =>
            ElementsToCopy.Clear();

        public void Paste(DialogueGraphView graphView)
        {
            var mapping = new Dictionary<string, string>();
            var copiedElements = new List<GraphElement>(ElementsToCopy.Count);
                
            foreach (var node in CreateDialogueNodesFrom(ElementsToCopy, mapping))
            {
                copiedElements.Add(node);
                graphView.AddToSelection(node);
            }

            foreach (var edge in CreateEdgesFrom(ElementsToCopy, mapping))
            {
                copiedElements.Add(edge);
                graphView.AddElement(edge);
                graphView.AddToSelection(edge);
            }
            
            _undoRegister.Register(new AddElements(graphView, copiedElements));
        }

        private IEnumerable<DialogueNodeView> CreateDialogueNodesFrom(IEnumerable<GraphElement> elementsToCopy, IDictionary<string, string> mapping)
        {
            foreach (var element in ElementsToCopy)
            {
                if (element is not DialogueNodeView nodeView) 
                    continue;
                
                var model = Copy(nodeView.Model);
                var node = _factory.CreateDialogue(model);
                mapping.Add(nodeView.Model.Guid, model.Guid);
                yield return node;
            }
        }

        private IEnumerable<Edge> CreateEdgesFrom(IEnumerable<GraphElement> elementsToCopy, IReadOnlyDictionary<string, string> mapping)
        {
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
                    yield return newEdge;
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