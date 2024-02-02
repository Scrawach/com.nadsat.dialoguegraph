using System;
using System.Collections.Generic;
using System.Linq;
using Editor.AssetManagement;
using Editor.Drawing.Nodes;
using Editor.Factories.NodeListeners;
using Editor.Windows.Variables;
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
        private readonly VariablesProvider _variables;
        private readonly GraphView _canvas;

        public NodeViewFactory(PersonRepository persons, PhraseRepository phrases, GraphView canvas, IDialogueNodeListener listener,
            ChoicesRepository choices, VariablesProvider variables)
        {
            _persons = persons;
            _phrases = phrases;
            _canvas = canvas;
            _listener = listener;
            _choices = choices;
            _variables = variables;
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
            view.PortRemoved += OnPortRemoved();
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
            view.PortRemoved += OnPortRemoved();
            return BindAndPlace(view, node);
        }

        private Action<IEnumerable<Port>> OnPortRemoved() =>
            (elements) =>
            {
                foreach (var element in elements)
                {
                    _canvas.RemoveElement(element);

                    foreach (var connection in element.connections.ToArray())
                    {
                        connection.input.Disconnect(connection);
                        connection.output.Disconnect(connection);
                        _canvas.RemoveElement(connection);
                    }
                }
            };

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