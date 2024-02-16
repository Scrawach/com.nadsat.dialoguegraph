using Editor.Undo;
using NUnit.Framework;

namespace Editor.Tests
{
    public class UndoHistoryTests
    {
        [Test]
        public void WhenAddCommand_ThenShouldPointer_EqualZero()
        {
            // arrange
            var undoHistory = new UndoStack();

            // act
            undoHistory.Add(new NopeCommand(1));

            // answer
            Assert.IsTrue(undoHistory.Pointer == 0);
        }

        [Test]
        public void WhenAddFewCommands_ThenShouldPerformUndo_InCorrectSequence()
        {
            // arrange
            var undoStack = new UndoStack();
            var firstCommand = new NopeCommand(1);
            var secondCommand = new NopeCommand(2);

            // act
            undoStack.Add(firstCommand);
            undoStack.Add(secondCommand);

            // answer
            Assert.AreEqual(undoStack.NextUndoOrDefault(), secondCommand);
            Assert.AreEqual(undoStack.NextUndoOrDefault(), firstCommand);
        }

        [Test]
        public void WhenAddFewCommands_AndUndoOne_AndAddAnotherOne_ThenShouldSkipUndoCommand()
        {
            // arrange
            var undoStack = new UndoStack();
            var first = new NopeCommand(1);
            var second = new NopeCommand(2);
            var third = new NopeCommand(3);
            var four = new NopeCommand(4);

            // act
            undoStack.Add(first);
            undoStack.Add(second);
            undoStack.Add(third);
            undoStack.NextUndoOrDefault();
            undoStack.Add(four);

            // answer
            Assert.AreEqual(undoStack.NextUndoOrDefault(), four);
            Assert.AreEqual(undoStack.NextUndoOrDefault(), second);
            Assert.AreEqual(undoStack.NextUndoOrDefault(), first);
        }

        public class NopeCommand : IUndoCommand
        {
            public readonly int Id;

            public NopeCommand(int id) =>
                Id = id;

            public void Undo() { }

            public void Redo() { }

            public override string ToString() =>
                Id.ToString();
        }
    }
}