using System.Linq;
using Editor.Drawing.Nodes;
using Editor.Undo;
using Editor.Undo.Commands;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Editor.Manipulators
{
    public class GraphViewUndoManipulator : Manipulator
    {
        private readonly IUndoRegister _undoRegister;
        private GraphView _graphView;

        public GraphViewUndoManipulator(IUndoRegister undoRegister) =>
            _undoRegister = undoRegister;

        protected override void RegisterCallbacksOnTarget()
        {
            _graphView = (GraphView) target;
            _graphView.graphViewChanged += OnGraphViewChanged;
        }

        protected override void UnregisterCallbacksFromTarget() =>
            _graphView.graphViewChanged -= OnGraphViewChanged;

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.movedElements != null)
            {
                var movableNodes = graphViewChange.movedElements.OfType<IMovableNode>().ToArray();
                var moveCommand = new MoveNodes(movableNodes);
                _undoRegister.Register(moveCommand);
                foreach (var element in movableNodes)
                    element.SavePosition(element.GetPosition());
            }

            if (graphViewChange.edgesToCreate != null) _undoRegister.Register(new CreateEdges(_graphView, graphViewChange.edgesToCreate));

            if (graphViewChange.elementsToRemove != null) _undoRegister.Register(new RemoveElements(_graphView, graphViewChange.elementsToRemove));

            return graphViewChange;
        }
    }
}