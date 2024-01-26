namespace Editor.Undo.Commands
{
    public class CompositeCommand : IUndoCommand
    {
        private readonly IUndoCommand[] _commands;

        public CompositeCommand(params IUndoCommand[] commands) =>
            _commands = commands;
        
        public void Undo()
        {
            foreach (var command in _commands) 
                command.Undo();
        }

        public void Redo()
        {
            foreach (var command in _commands) 
                command.Redo();
        }
    }
}