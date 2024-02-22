using Runtime.Nodes;

namespace Editor.Drawing.Nodes
{
    public interface IModelHandle : IRootable
    {
        BaseDialogueNode Model { get; }
    }
}