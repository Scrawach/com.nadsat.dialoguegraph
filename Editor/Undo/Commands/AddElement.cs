using UnityEditor.Experimental.GraphView;

namespace Nadsat.DialogueGraph.Editor.Undo.Commands
{
    public class AddElement : IUndoCommand
    {
        private readonly GraphView _canvas;
        private readonly GraphElement _graphElement;

        public AddElement(GraphElement graphElement, GraphView canvas)
        {
            _graphElement = graphElement;
            _canvas = canvas;
        }

        public void Undo() =>
            _canvas.RemoveElement(_graphElement);

        public void Redo() =>
            _canvas.AddElement(_graphElement);

        public override string ToString() =>
            $"Add {_graphElement} to {_canvas}";
    }
}