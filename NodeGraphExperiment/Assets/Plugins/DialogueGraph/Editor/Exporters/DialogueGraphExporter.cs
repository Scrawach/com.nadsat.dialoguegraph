using Nadsat.DialogueGraph.Editor.Data;
using Nadsat.DialogueGraph.Editor.Serialization;

namespace Nadsat.DialogueGraph.Editor.Exporters
{
    public class DialogueGraphExporter
    {
        private readonly DialogueGraphSerializer _graphSerializer;
        private readonly CsvExporter _csvExporter;
        private readonly DialoguesProvider _dialogues;

        public DialogueGraphExporter(DialogueGraphSerializer graphSerializer, CsvExporter csvExporter, DialoguesProvider dialogues)
        {
            _graphSerializer = graphSerializer;
            _csvExporter = csvExporter;
            _dialogues = dialogues;
        }

        public void Export()
        {
            var graph = _graphSerializer.Serialize();
            var assetPath = _dialogues.GetDialoguePath(graph.Name);
            _dialogues.CreateNewDialogue(graph, assetPath);
            
            var pathToFolder = _dialogues.GetDialogueFolder(graph.Name);
            _csvExporter.Export(pathToFolder);
        }
    }
}