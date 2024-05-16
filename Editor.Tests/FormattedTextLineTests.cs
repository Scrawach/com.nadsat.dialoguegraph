using Nadsat.DialogueGraph.Editor.Data;
using NUnit.Framework;

namespace Plugins.DialogueGraph.Editor.Tests
{
    public class FormattedTextLineTests
    {
        [TestCase("text", "text")]
        [TestCase("te-t", "te-t")]
        [TestCase("te--t", "te—t")]
        [TestCase("a -- b", "a — b")]
        [TestCase("<<b>>", "«b»")]
        public void WhenConvertFormattedTextLineToString_ThenShouldReturnLineWithReplaceSpecificSymbols(string input, string expected)
        {
            // assign
            var line = new FormattedTextLine(input);
            
            // act
            var result = line.ToString();
            
            // assert
            Assert.AreEqual(expected, result);
        }
    }
}