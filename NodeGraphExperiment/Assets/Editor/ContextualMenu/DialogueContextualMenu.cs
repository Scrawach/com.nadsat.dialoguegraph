using Editor.Drawing.Nodes;
using Editor.Factories.NodeListeners;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Editor.ContextualMenu
{
    public class DialogueContextualMenu : Manipulator
    {
        private readonly NodesProvider _provider;
        private readonly NodesCreationMenuBuilder _nodesCreationMenu;

        public DialogueContextualMenu(NodesProvider provider, NodesCreationMenuBuilder nodesCreationMenu)
        {
            _provider = provider;
            _nodesCreationMenu = nodesCreationMenu;
        }
        
        protected override void RegisterCallbacksOnTarget() =>
            target.RegisterCallback<ContextualMenuPopulateEvent>(OnContextualMenuBuild);

        protected override void UnregisterCallbacksFromTarget() =>
            target.UnregisterCallback<ContextualMenuPopulateEvent>(OnContextualMenuBuild);

        private void OnContextualMenuBuild(ContextualMenuPopulateEvent evt)
        {
            if (evt.target is not GraphView)
            {
                if (evt.target is DialogueNodeView nodeView)
                {
                    evt.menu.InsertAction(0, "Set as Root", _ =>
                    {
                        _provider.MarkAsRootNode(nodeView);
                    });
                    
                    evt.menu.AppendSeparator();
                }
                return;
            }
            
            _nodesCreationMenu.Build(evt);
            
            //evt.menu.AppendAction("Create Group", (action) => _factory.CreateGroup(at: action.eventInfo.mousePosition));
            //evt.menu.AppendAction("Create Sticky Note", (action) => _factory.CreateStickyNote(at: action.eventInfo.mousePosition));
        }
    }
}