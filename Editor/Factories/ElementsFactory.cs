using Nadsat.DialogueGraph.Editor.Drawing.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Factories
{
    public class ElementsFactory
    {
        private readonly GraphView _canvas;

        public ElementsFactory(GraphView canvas) =>
            _canvas = canvas;

        public Group CreateGroup(Vector2 at)
        {
            var group = new Group();
            var worldPosition = _canvas.contentViewContainer.WorldToLocal(at);
            group.SetPosition(new Rect(worldPosition, Vector2.zero));
            _canvas.AddElement(group);
            return group;
        }

        public StickyNoteView CreateStickyNote(Vector2 at)
        {
            var stickyNote = new StickyNoteView();
            var worldPosition = _canvas.contentViewContainer.WorldToLocal(at);
            stickyNote.SetPosition(new Rect(worldPosition, Vector2.zero));
            _canvas.AddElement(stickyNote);
            return stickyNote;
        }
    }
}