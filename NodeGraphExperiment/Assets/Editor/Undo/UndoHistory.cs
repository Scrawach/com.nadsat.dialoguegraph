using System.Collections.Generic;
using UnityEngine;

namespace Editor.Undo
{
    public class UndoHistory : IUndoHistory, IUndoRegister
    {
        private readonly List<IUndoCommand> _commands = new();
        private int _pointer = -1;

        public void Register(IUndoCommand command)
        {
            _pointer++;

            if (_pointer < _commands.Count - 1) 
                _commands.RemoveRange(_pointer, _commands.Count - _pointer);

            _commands.Add(command);
            Debug.Log($"{command}");
        }
        
        public void Undo()
        {
            Debug.Log($"UNDO: {_pointer}, {_commands.Count}");
            if (_pointer < 0)
                return;
            
            var command = _commands[_pointer];
            command.Undo();
            _pointer--;
        }

        public void Redo()
        {
            Debug.Log($"Redo: {_pointer}, {_commands.Count}");
            if (_pointer > _commands.Count - 2)
                return;

            _pointer++;
            var command = _commands[_pointer];
            command.Redo();
        }
    }
}