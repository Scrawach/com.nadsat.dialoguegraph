using Nadsat.DialogueGraph.Runtime.Nodes;

namespace Nadsat.DialogueGraph.Editor.Drawing.Nodes
{
    public interface IModelHandle : IRootable
    {
        BaseDialogueNode Model { get; }
    }
}