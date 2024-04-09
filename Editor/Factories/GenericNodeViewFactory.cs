using Nadsat.DialogueGraph.Editor.Drawing.Inspector;
using Nadsat.DialogueGraph.Editor.Drawing.Nodes;
using Nadsat.DialogueGraph.Editor.Manipulators;
using Nadsat.DialogueGraph.Editor.Windows;
using Nadsat.DialogueGraph.Runtime.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Factories
{
    public class GenericNodeViewFactory
    {
        private readonly DialogueGraphWindow _root;
        private readonly GraphView _canvas;
        private readonly InspectorViewFactory _inspectorFactory;
        private readonly InspectorView _inspectorView;

        public GenericNodeViewFactory(DialogueGraphWindow root, GraphView canvas,
            InspectorView inspectorView, InspectorViewFactory inspectorFactory)
        {
            _root = root;
            _canvas = canvas;
            _inspectorView = inspectorView;
            _inspectorFactory = inspectorFactory;
        }

        public TNodeView Create<TNodeView, TModel>(TNodeView view, TModel model)
            where TNodeView : BaseNodeView<TModel>
            where TModel : BaseDialogueNode
        {
            view.Initialize();
            view.Bind(model);
            view.SetPosition(model.Position);
            view.AddManipulator(new InspectorShowManipulator(_inspectorView, _inspectorFactory));
            view.AddManipulator(new NodeChangesDirtyMarkManipulator(_root, model));
            _canvas.AddElement(view);
            _canvas.ClearSelection();
            _canvas.AddToSelection(view);
            _root.IsDirty = true;
            return view;
        }
    }
}