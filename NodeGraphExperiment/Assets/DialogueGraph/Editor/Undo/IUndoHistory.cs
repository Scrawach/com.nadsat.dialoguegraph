namespace Editor.Undo
{
    public interface IUndoHistory
    {
        void Undo();
        void Redo();
    }
}