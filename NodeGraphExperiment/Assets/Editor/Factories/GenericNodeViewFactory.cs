using Editor.Drawing.Inspector;
using Editor.Drawing.Nodes;
using Editor.Manipulators;
using Runtime.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Editor.Factories
{
    public class GenericNodeViewFactory
    {
        private readonly GraphView _canvas;
        private readonly InspectorViewFactory _inspectorFactory;
        private readonly InspectorView _inspectorView;

        public GenericNodeViewFactory(GraphView canvas, InspectorView inspectorView, InspectorViewFactory inspectorFactory)
        {
            _canvas = canvas;
            _inspectorView = inspectorView;
            _inspectorFactory = inspectorFactory;
        }

        public TNodeView Create<TNodeView, TModel>(TNodeView view, TModel model)
            where TNodeView : BaseNodeView<TModel>
            where TModel : BaseDialogueNode
        {
            view.AddInputAndOutputPorts();
            view.Bind(model);
            view.SetPosition(model.Position);
            view.AddManipulator(new InspectorShowManipulator(_inspectorView, _inspectorFactory));
            _canvas.AddElement(view);
            _canvas.AddToSelection(view);
            return view;
        }
    }
}