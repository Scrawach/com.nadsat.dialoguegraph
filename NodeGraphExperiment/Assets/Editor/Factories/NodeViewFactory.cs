using Editor.AssetManagement;
using Editor.Drawing.Nodes;
using Editor.Factories.NodeListeners;
using Runtime.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Factories
{
    public class NodeViewFactory : INodeViewFactory
    {
        private readonly PersonRepository _persons;
        private readonly PhraseRepository _phrases;
        private readonly IDialogueNodeListener _listener;
        private readonly GraphView _canvas;

        public NodeViewFactory(PersonRepository persons, PhraseRepository phrases, GraphView canvas, IDialogueNodeListener listener)
        {
            _persons = persons;
            _phrases = phrases;
            _canvas = canvas;
            _listener = listener;
        }
        
        public DialogueNodeView CreateDialogue(DialogueNode node)
        {
            var view = new DialogueNodeView(_phrases, _persons);
            view.AddInputAndOutputPorts();
            
            view.Bind(node);
            view.SetPosition(node.Position);
            
            _listener.Register(view);
            _canvas.AddElement(view);
            return view;
        }

        public RedirectNodeView CreateRedirect(RedirectNode node)
        {
            var redirectNode = new RedirectNodeView {title = "",};
            redirectNode.AddInputAndOutputPorts();
            
            redirectNode.Bind(node);
            redirectNode.SetPosition(node.Position);
            
            redirectNode.styleSheets.Add(Resources.Load<StyleSheet>("Styles/RedirectNode"));
            
            _canvas.AddElement(redirectNode);
            return redirectNode;
        }
    }
}