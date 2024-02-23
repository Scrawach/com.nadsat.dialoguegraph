using System.Collections.Generic;

namespace Nadsat.DialogueGraph.Editor.Undo
{
    public class UndoStack
    {
        private readonly List<IUndoCommand> _commands = new();
        public int Pointer { get; private set; } = -1;

        public void Add(IUndoCommand command)
        {
            Pointer++;

            if (Pointer < _commands.Count) _commands.RemoveRange(Pointer, _commands.Count - Pointer);

            _commands.Add(command);
        }

        public IUndoCommand NextUndoOrDefault()
        {
            if (Pointer < 0)
                return null;

            var command = _commands[Pointer];
            Pointer--;
            return command;
        }

        public IUndoCommand NextRedoOrDefault()
        {
            if (Pointer > _commands.Count - 2)
                return null;

            Pointer++;
            return _commands[Pointer];
        }
    }
}