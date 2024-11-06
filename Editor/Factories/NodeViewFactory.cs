using Nadsat.DialogueGraph.Editor.AssetManagement;
using Nadsat.DialogueGraph.Editor.Drawing.Nodes;
using Nadsat.DialogueGraph.Editor.Manipulators.GraphViewManipulators;
using Nadsat.DialogueGraph.Editor.Windows.Variables;
using Nadsat.DialogueGraph.Runtime.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Factories
{
    public class NodeViewFactory : INodeViewFactory
    {
        private readonly GenericNodeViewFactory _baseFactory;

        private readonly GraphView _canvas;
        private readonly ChoicesRepository _choices;
        private readonly DialogueNodeViewFactory _dialogueFactory;
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

        public BackgroundImageNodeView CreateBackgroundImageNode(BackgroundImageNode node)
        {
            var view = new BackgroundImageNodeView();
            return _baseFactory.Create(view, node);
        }

        public InterludeNodeView CreateInterlude(InterludeNode node) => 
            _dialogueFactory.CreateInterlude(node);

        public PopupPhraseNodeView CreatePopup(PopupPhraseNode node) =>
            _dialogueFactory.CreatePopup(node);

        public PlacementNodeView CreatePlacement(PlacementNode node)
        {
            var placementNode = new PlacementNodeView();
            return _baseFactory.Create(placementNode, node);
        }

        public RedirectNodeView CreateRedirect(RedirectNode node)
        {
            var redirectNode = new RedirectNodeView {title = ""};
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

        public AudioEventNodeView CreateAudioEvent(AudioEventNode node)
        {
            var view = new AudioEventNodeView();
            return _baseFactory.Create(view, node);
        }

        public EndNodeView CreateEnd(EndNode node)
        {
            var view = new EndNodeView();
            return _baseFactory.Create(view, node);
        }

        public SwitchNodeView CreateSwitch(SwitchNode node)
        {
            var view = new SwitchNodeView();
            view.AddManipulator(new RemovePortsListener(_canvas));
            return _baseFactory.Create(view, node);
        }
    }
}