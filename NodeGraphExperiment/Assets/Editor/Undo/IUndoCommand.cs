namespace Editor.Undo
{
    public interface IUndoCommand
    {
        void Undo();
        void Redo();
    }
}