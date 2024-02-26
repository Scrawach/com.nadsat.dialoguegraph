using Nadsat.DialogueGraph.Editor.Drawing.Nodes;
using Nadsat.DialogueGraph.Editor.Undo;
using Nadsat.DialogueGraph.Editor.Undo.Commands;
using Nadsat.DialogueGraph.Runtime.Nodes;
using UnityEditor.Experimental.GraphView;

namespace Nadsat.DialogueGraph.Editor.Factories
{
    public class UndoNodeViewFactory : INodeViewFactory
    {
        private readonly INodeViewFactory _factory;
        private readonly GraphView _graphView;
        private readonly IUndoRegister _undoRegister;

        public UndoNodeViewFactory(INodeViewFactory factory, IUndoRegister undoRegister, GraphView graphView)
        {
            _factory = factory;
            _undoRegister = undoRegister;
            _graphView = graphView;
        }

        public DialogueNodeView CreateDialogue(DialogueNode node) =>
            RegisterCreated(_factory.CreateDialogue(node));

        public RedirectNodeView CreateRedirect(RedirectNode node) =>
            RegisterCreated(_factory.CreateRedirect(node));

        public ChoicesNodeView CreateChoices(ChoicesNode node) =>
            RegisterCreated(_factory.CreateChoices(node));

        public SwitchNodeView CreateSwitch(SwitchNode node) =>
            RegisterCreated(_factory.CreateSwitch(node));

        public VariableNodeView CreateVariable(VariableNode node) =>
            RegisterCreated(_factory.CreateVariable(node));

        public AudioEventNodeView CreateAudioEvent(AudioEventNode node) =>
            RegisterCreated(_factory.CreateAudioEvent(node));

        private TViewNode RegisterCreated<TViewNode>(TViewNode node) where TViewNode : GraphElement
        {
            _undoRegister.Register(new AddElement(node, _graphView));
            return node;
        }
    }
}