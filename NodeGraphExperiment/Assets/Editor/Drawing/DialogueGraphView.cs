using System.Collections.Generic;
using System.Linq;
using Editor.Drawing.Nodes;
using Editor.Factories;
using Editor.Undo;
using Editor.Undo.Commands;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Drawing
{
    public class DialogueGraphView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<DialogueGraphView, UxmlTraits> { }
        
        private RedirectNodeFactory _redirectFactory;
        private IUndoRegister _undoRegister;

        public DialogueGraphView()
        {
            EditorPrefs.SetBool("GraphSnapping", false);
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer() { maxScale = 2f, minScale = 0.1f});
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            var stylesheet = Resources.Load<StyleSheet>("Styles/DialogueGraph");
            styleSheets.Add(stylesheet);
            RegisterCallback<MouseDownEvent>(OnMouseDown, TrickleDown.TrickleDown);

            graphViewChanged = OnGraphViewChanged;
        }

        public void Initialize(RedirectNodeFactory redirectNodeFactory, IUndoRegister undoRegister)
        {
            _undoRegister = undoRegister;
            _redirectFactory = redirectNodeFactory;
        }

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

            if (graphViewChange.edgesToCreate != null)
            {
                _undoRegister.Register(new CreateEdges(this, graphViewChange.edgesToCreate));
            }

            if (graphViewChange.elementsToRemove != null)
            {
                _undoRegister.Register(new RemoveElements(this, graphViewChange.elementsToRemove));
            }

            return graphViewChange;
        }

        private void OnMouseDown(MouseDownEvent evt)
        {
            if (evt.clickCount >= 2 && evt is { target: Edge edge }) 
                _redirectFactory.CreateRedirect(edge, evt.mousePosition, OnMouseDown);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) =>
            ports.ToList();

        public void Find(Node view)
        {
            var viewPosition = view.GetPosition();
            MoveTo(viewPosition);
            
            ClearSelection();
            AddToSelection(view);
        }

        public void MoveTo(Rect target)
        {
            CalculateFrameTransform(target, layout, 0, out var translation, out var scaling);
            UpdateViewTransform(translation, scaling);
        }
    }
}