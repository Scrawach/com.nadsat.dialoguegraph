using Nadsat.DialogueGraph.Runtime.Nodes;

namespace Nadsat.DialogueGraph.Editor.Drawing.Nodes.Abstract
{
    public interface IModelHandle : IRootable
    {
        BaseDialogueNode Model { get; }
    }
}