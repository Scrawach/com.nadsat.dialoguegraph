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
        private readonly ChoicesRepository _choices;
        private readonly GraphView _canvas;

        public NodeViewFactory(PersonRepository persons, PhraseRepository phrases, GraphView canvas, IDialogueNodeListener listener,
            ChoicesRepository choices)
        {
            _persons = persons;
            _phrases = phrases;
            _canvas = canvas;
            _listener = listener;
            _choices = choices;
        }
        
        public DialogueNodeView CreateDialogue(DialogueNode node)
        {
            var view = new DialogueNodeView(_phrases, _persons);
            view = BindAndPlace(view, node);
            return view;
        }

        public RedirectNodeView CreateRedirect(RedirectNode node)
        {
            var redirectNode = new RedirectNodeView {title = "",};
            redirectNode.styleSheets.Add(Resources.Load<StyleSheet>("Styles/RedirectNode"));
            return BindAndPlace(redirectNode, node);
        }

        public ChoicesNodeView CreateChoices(ChoicesNode choices)
        {
            var view = new ChoicesNodeView(_choices);
            return BindAndPlace(view, choices);
        }

        public ChangeVariableNodeView CreateChangeVariable(ChangeVariableNode variable)
        {
            var view = new ChangeVariableNodeView();
            return BindAndPlace(view, variable);
        }

        public SwitchNodeView CreateSwitch(SwitchNode node)
        {
            var view = new SwitchNodeView();
            return BindAndPlace(view, node);
        }

        private TNodeView BindAndPlace<TNodeView, TModel>(TNodeView view, TModel model)
            where TNodeView : BaseNodeView<TModel>
            where TModel : BaseDialogueNode
        {
            view.AddInputAndOutputPorts();
            view.Bind(model);
            view.SetPosition(model.Position);
            _canvas.AddElement(view);
            _listener.Register(view);
            return view;
        }
    }
}