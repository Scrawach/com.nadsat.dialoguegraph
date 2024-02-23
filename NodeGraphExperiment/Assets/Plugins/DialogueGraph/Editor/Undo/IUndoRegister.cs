namespace Nadsat.DialogueGraph.Editor.Undo
{
    public interface IUndoRegister
    {
        void Register(IUndoCommand command);
    }
}