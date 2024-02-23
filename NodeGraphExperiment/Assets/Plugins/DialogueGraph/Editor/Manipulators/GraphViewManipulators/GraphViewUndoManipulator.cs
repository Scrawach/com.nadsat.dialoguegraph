using System.Linq;
using Nadsat.DialogueGraph.Editor.Drawing.Nodes;
using Nadsat.DialogueGraph.Editor.Undo;
using Nadsat.DialogueGraph.Editor.Undo.Commands;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Manipulators.GraphViewManipulators
{
    public class GraphViewUndoManipulator : Manipulator
    {
        private readonly IUndoRegister _undoRegister;
        private UnityEditor.Experimental.GraphView.GraphView _graphView;

        public GraphViewUndoManipulator(IUndoRegister undoRegister) =>
            _undoRegister = undoRegister;

        protected override void RegisterCallbacksOnTarget()
        {
            _graphView = (UnityEditor.Experimental.GraphView.GraphView) target;
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