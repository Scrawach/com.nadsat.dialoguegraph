namespace Editor.Undo
{
    public interface IUndoRegister
    {
        void Register(IUndoCommand command);
    }
}