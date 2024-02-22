namespace Editor.Undo
{
    public class UndoHistory : IUndoHistory, IUndoRegister
    {
        private readonly UndoStack _undoStack = new();

        public void Undo()
        {
            var command = _undoStack.NextUndoOrDefault();
            command?.Undo();
        }

        public void Redo()
        {
            var command = _undoStack.NextRedoOrDefault();
            command?.Redo();
        }

        public void Register(IUndoCommand command) =>
            _undoStack.Add(command);
    }
}