using Editor.AssetManagement;
using Editor.Drawing.Nodes;
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
        private readonly GenericNodeViewFactory _baseFactory;
        private readonly DialogueNodeViewFactory _dialogueFactory;
        
        private readonly GraphView _canvas;
        private readonly ChoicesRepository _choices;
        private readonly VariablesProvider _variables;

        public NodeViewFactory(GenericNodeViewFactory baseFactory, DialogueNodeViewFactory dialogueFactory, GraphView canvas,
            ChoicesRepository choices, VariablesProvider variables)
        {
            _baseFactory = baseFactory;
            _dialogueFactory = dialogueFactory;
            _canvas = canvas;
            _choices = choices;
            _variables = variables;
        }
        
        public DialogueNodeView CreateDialogue(DialogueNode node) =>
            _dialogueFactory.Create(node);

        public RedirectNodeView CreateRedirect(RedirectNode node)
        {
            var redirectNode = new RedirectNodeView {title = "",};
            redirectNode.styleSheets.Add(Resources.Load<StyleSheet>("Styles/RedirectNode"));
            return _baseFactory.Create(redirectNode, node);
        }

        public ChoicesNodeView CreateChoices(ChoicesNode choices)
        {
            var view = new ChoicesNodeView(_choices);
            view.AddManipulator(new RemovePortsListener(_canvas));
            return _baseFactory.Create(view, choices);
        }

        public VariableNodeView CreateVariable(VariableNode variable)
        {
            var view = new VariableNodeView(_variables);
            return _baseFactory.Create(view, variable);
        }

        public SwitchNodeView CreateSwitch(SwitchNode node)
        {
            var view = new SwitchNodeView();
            view.AddManipulator(new RemovePortsListener(_canvas));
            return _baseFactory.Create(view, node);
        }
    }
}