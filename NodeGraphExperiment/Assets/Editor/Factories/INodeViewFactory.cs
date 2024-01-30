using System.Collections.Generic;
using Editor.Drawing.Nodes;
using Runtime.Nodes;

namespace Editor.Factories
{
    public interface INodeViewFactory
    {
        DialogueNodeView CreateDialogue(DialogueNode node);
        RedirectNodeView CreateRedirect(RedirectNode node);
        ChoicesNodeView CreateChoices(ChoicesNode node);
        SwitchNodeView CreateSwitch(SwitchNode node);
        VariableNodeView CreateVariable(VariableNode node);
    }
}