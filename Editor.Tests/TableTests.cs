using Nadsat.DialogueGraph.Editor.Data;
using Nadsat.DialogueGraph.Editor.Localization;
using Nadsat.DialogueGraph.Runtime.Localization;
using NUnit.Framework;
using UnityEngine;

namespace Plugins.DialogueGraph.Editor.Tests
{
    public class TableTests
    {
        [Test]
        public void WhenGetNotExistKeyFromTable_ThenShouldReturnErrorLine()
        {
            // assign
            var csvText = new CsvText("Keys,Russian,English\na,1,2\nb,3");
            var table = new Table("test", csvText);
            
            // act
            var result = table.Get("English", "b");
            
            // assert
            Assert.AreEqual(Table.NotValidKey, result);
        }
        
        [Test]
        public void WhenGetByKeyFromNotFullLanguage_ThenShouldReturnValidValue()
        {
            // assign
            var csvText = new CsvText("Keys,Russian,English\na,1,2\nb,3");
            var table = new Table("test", csvText);
            
            // act
            var result = table.Get("English", "a");
            
            // assert
            Assert.AreEqual("2", result);
        }
    }
}