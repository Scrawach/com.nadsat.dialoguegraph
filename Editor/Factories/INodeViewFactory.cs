using Nadsat.DialogueGraph.Editor.Drawing.Nodes;
using Nadsat.DialogueGraph.Runtime.Nodes;

namespace Nadsat.DialogueGraph.Editor.Factories
{
    public interface INodeViewFactory
    {
        DialogueNodeView CreateDialogue(DialogueNode node);
        InterludeNodeView CreateInterlude(InterludeNode node);
        PopupPhraseNodeView CreatePopup(PopupPhraseNode node);
        PlacementNodeView CreatePlacement(PlacementNode node);
        RedirectNodeView CreateRedirect(RedirectNode node);
        ChoicesNodeView CreateChoices(ChoicesNode node);
        SwitchNodeView CreateSwitch(SwitchNode node);
        VariableNodeView CreateVariable(VariableNode node);
        AudioEventNodeView CreateAudioEvent(AudioEventNode node);
        EndNodeView CreateEnd(EndNode node);
    }
}