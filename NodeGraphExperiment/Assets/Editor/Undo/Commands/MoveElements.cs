using System.Collections.Generic;
using System.Linq;
using Editor.Drawing.Nodes;
using UnityEditor.Experimental.GraphView;

namespace Editor.Undo.Commands
{
    public class MoveElements : IUndoCommand
    {
        private readonly CompositeCommand _moveCommand;

        public MoveElements(IEnumerable<GraphElement> elements)
        {
            var array = elements.ToArray();
            _moveCommand = new CompositeCommand
            (
                new MoveDialogueNodes(array.OfType<DialogueNodeView>()),
                new MoveRedirectNodes(array.OfType<RedirectNodeView>())
            );
        }
        
        public void Undo() =>
            _moveCommand.Undo();

        public void Redo() =>
            _moveCommand.Redo();
    }
}