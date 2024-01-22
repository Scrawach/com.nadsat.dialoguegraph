using System.Collections.Generic;

namespace Editor.Undo
{
    public class UndoStack
    {
        private readonly List<IUndoCommand> _commands = new();
        private int _pointer = -1;
        public int Pointer => _pointer;

        public void Add(IUndoCommand command)
        {
            _pointer++;

            if (_pointer < _commands.Count)
            {
                _commands.RemoveRange(_pointer, _commands.Count - _pointer);
            }

            _commands.Add(command);
        }
        
        public IUndoCommand NextUndoOrDefault()
        {
            if (_pointer < 0)
                return null;
            
            var command = _commands[_pointer];
            _pointer--;
            return command;
        }

        public IUndoCommand NextRedoOrDefault()
        {
            if (_pointer > _commands.Count - 2)
                return null;

            _pointer++;
            return _commands[_pointer];
        }
    }
}