using UnityEditor.Experimental.GraphView;

namespace Editor.Undo.Commands
{
    public class RemoveGraphElement : IUndoCommand
    {
        private readonly GraphView _canvas;
        private readonly GraphElement _graphElement;

        public RemoveGraphElement(GraphView canvas, GraphElement graphElement)
        {
            _canvas = canvas;
            _graphElement = graphElement;
        }
        
        public void Undo() =>
            _canvas.AddElement(_graphElement);

        public void Redo() =>
            _canvas.RemoveElement(_graphElement);
    }
}