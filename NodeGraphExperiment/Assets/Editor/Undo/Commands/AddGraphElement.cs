using UnityEditor.Experimental.GraphView;

namespace Editor.Undo.Commands
{
    public class AddGraphElement : IUndoCommand
    {
        private readonly GraphElement _graphElement;
        private readonly GraphView _canvas;

        public AddGraphElement(GraphElement graphElement, GraphView canvas)
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