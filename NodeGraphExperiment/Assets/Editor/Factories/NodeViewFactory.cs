using Editor.AssetManagement;
using Editor.Drawing.Nodes;
using Editor.Factories.NodeListeners;
using Editor.Manipulators;
using Editor.Windows.Variables;
using Runtime.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Factories
{
    public class NodeViewFactory : INodeViewFactory
    {
        private readonly DialogueDatabase _persons;
        private readonly PhraseRepository _phrases;
        private readonly IDialogueNodeListener _listener;
        private readonly ChoicesRepository _choices;
        private readonly VariablesProvider _variables;
        private readonly InspectorViewFactory _inspectorFactory;
        private readonly GraphView _canvas;

        public NodeViewFactory(DialogueDatabase persons, PhraseRepository phrases, GraphView canvas, IDialogueNodeListener listener,
            ChoicesRepository choices, VariablesProvider variables, InspectorViewFactory inspectorFactory)
        {
            _persons = persons;
            _phrases = phrases;
            _canvas = canvas;
            _listener = listener;
            _choices = choices;
            _variables = variables;
            _inspectorFactory = inspectorFactory;
        }
        
        public DialogueNodeView CreateDialogue(DialogueNode node)
        {
            var view = new DialogueNodeView(_phrases, _persons);
            view = BindAndPlace(view, node);

            if (IsPersonWithoutPhrase(node))
            {
                node.SetPhraseId(_phrases.Create(node.PersonId));
                _inspectorFactory.StartEditPhrase(node.PhraseId);
            }

            return view;
        }

        private static bool IsPersonWithoutPhrase(DialogueNode node) =>
            !string.IsNullOrWhiteSpace(node.PersonId) && string.IsNullOrWhiteSpace(node.PhraseId);

        public RedirectNodeView CreateRedirect(RedirectNode node)
        {
            var redirectNode = new RedirectNodeView {title = "",};
            redirectNode.styleSheets.Add(Resources.Load<StyleSheet>("Styles/RedirectNode"));
            return BindAndPlace(redirectNode, node);
        }

        public ChoicesNodeView CreateChoices(ChoicesNode choices)
        {
            var view = new ChoicesNodeView(_choices);
            view.AddManipulator(new RemovePortsListener(_canvas));
            return BindAndPlace(view, choices);
        }

        public VariableNodeView CreateVariable(VariableNode variable)
        {
            var view = new VariableNodeView(_variables);
            return BindAndPlace(view, variable);
        }

        public SwitchNodeView CreateSwitch(SwitchNode node)
        {
            var view = new SwitchNodeView();
            view.AddManipulator(new RemovePortsListener(_canvas));
            return BindAndPlace(view, node);
        }
        
        private TNodeView BindAndPlace<TNodeView, TModel>(TNodeView view, TModel model)
            where TNodeView : BaseNodeView<TModel>
            where TModel : BaseDialogueNode
        {
            view.AddInputAndOutputPorts();
            view.Bind(model);
            view.SetPosition(model.Position);
            _listener.Register(view);
            _canvas.AddElement(view);
            _canvas.AddToSelection(view);
            return view;
        }
    }
}