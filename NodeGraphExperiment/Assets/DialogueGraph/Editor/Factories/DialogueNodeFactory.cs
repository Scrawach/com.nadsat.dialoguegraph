using System;
using DialogueGraph.Editor.AssetManagement;
using DialogueGraph.Editor.Drawing.Nodes;
using DialogueGraph.Runtime;
using UnityEditor.Experimental.GraphView;

namespace DialogueGraph.Editor.Factories
{
    public class DialogueNodeFactory
    {
        private readonly PersonRepository _persons;
        private readonly PhraseRepository _phrases;
        private readonly EditorAssets _assets;
        private readonly GraphView _canvas;

        public DialogueNodeFactory(PersonRepository persons, PhraseRepository phrases, EditorAssets assets, GraphView canvas)
        {
            _persons = persons;
            _phrases = phrases;
            _assets = assets;
            _canvas = canvas;
        }

        public DialogueNodeView CreateNew() =>
            CreateFrom(new DialogueNode { Guid = Guid.NewGuid().ToString() });

        public DialogueNodeView CreateFrom(DialogueNode data)
        {
            var view = new DialogueNodeView(_phrases, _persons, _assets);
            view.Bind(data);
            _canvas.AddElement(view);
            return view;
        }
    }
}