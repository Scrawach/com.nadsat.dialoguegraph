using Editor.Drawing.Nodes;
using Editor.Undo;
using Editor.Undo.Commands;
using Runtime.Nodes;
using UnityEditor.Experimental.GraphView;

namespace Editor.Factories
{
    public class UndoNodeViewFactory : INodeViewFactory
    {
        private readonly INodeViewFactory _factory;
        private readonly IUndoRegister _undoRegister;
        private readonly GraphView _graphView;

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

        private TViewNode RegisterCreated<TViewNode>(TViewNode node) where TViewNode : GraphElement
        {
            _undoRegister.Register(new AddElement(node, _graphView));
            return node;
        }
    }
}