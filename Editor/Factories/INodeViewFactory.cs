using Nadsat.DialogueGraph.Editor.Drawing.Nodes;
using Nadsat.DialogueGraph.Runtime.Nodes;

namespace Nadsat.DialogueGraph.Editor.Factories
{
    public interface INodeViewFactory
    {
        DialogueNodeView CreateDialogue(DialogueNode node);
        RedirectNodeView CreateRedirect(RedirectNode node);
        ChoicesNodeView CreateChoices(ChoicesNode node);
        SwitchNodeView CreateSwitch(SwitchNode node);
        VariableNodeView CreateVariable(VariableNode node);
        AudioEventNodeView CreateAudioEvent(AudioEventNode node);
    }
}