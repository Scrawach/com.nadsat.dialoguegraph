using Nadsat.DialogueGraph.Editor.Drawing.Nodes;
using Nadsat.DialogueGraph.Editor.Factories;
using Nadsat.DialogueGraph.Editor.Factories.NodeListeners;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.ContextualMenu
{
    public class DialogueContextualMenu : Manipulator
    {
        private readonly ElementsFactory _elementsFactory;
        private readonly NodesCreationMenuBuilder _nodesCreationMenu;
        private readonly NodesProvider _provider;

        public DialogueContextualMenu(NodesProvider provider, NodesCreationMenuBuilder nodesCreationMenu, ElementsFactory elementsFactory)
        {
            _provider = provider;
            _nodesCreationMenu = nodesCreationMenu;
            _elementsFactory = elementsFactory;
        }

        protected override void RegisterCallbacksOnTarget() =>
            target.RegisterCallback<ContextualMenuPopulateEvent>(OnContextualMenuBuild);

        protected override void UnregisterCallbacksFromTarget() =>
            target.UnregisterCallback<ContextualMenuPopulateEvent>(OnContextualMenuBuild);

        private void OnContextualMenuBuild(ContextualMenuPopulateEvent evt)
        {
            if (evt.target is not GraphView)
            {
                if (evt.target is IModelHandle nodeView)
                {
                    evt.menu.InsertAction(0, "Set as Root", _ => { _provider.MarkAsRootNode(nodeView); });

                    evt.menu.AppendSeparator();
                }

                return;
            }

            _nodesCreationMenu.Build(evt);
            evt.menu.InsertAction(2, "Create Note", action => _elementsFactory.CreateStickyNote(action.eventInfo.mousePosition));
            //evt.menu.AppendAction("Create Group", (action) => _factory.CreateGroup(at: action.eventInfo.mousePosition));
            //evt.menu.AppendAction("Create Sticky Note", (action) => _factory.CreateStickyNote(at: action.eventInfo.mousePosition));
        }
    }
}