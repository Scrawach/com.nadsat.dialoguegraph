using Nadsat.DialogueGraph.Editor.Data;
using Nadsat.DialogueGraph.Editor.Serialization;
using Nadsat.DialogueGraph.Editor.Serialization.Exporters;

namespace Nadsat.DialogueGraph.Editor.Backup
{
    public class BackupGraphExporter
    {
        private readonly DialogueGraphSerializer _graphSerializer;
        private readonly CsvExporter _csvExporter;
        private readonly DialoguesProvider _dialogues;

        public BackupGraphExporter(DialogueGraphSerializer graphSerializer, CsvExporter csvExporter, DialoguesProvider dialogues)
        {
            _graphSerializer = graphSerializer;
            _csvExporter = csvExporter;
            _dialogues = dialogues;
        }

        public void Export()
        {
            var graph = _graphSerializer.Serialize();
            var assetPath = _dialogues.GetBackupDialoguePath(graph.Name);
            _dialogues.CreateNewDialogue(graph, assetPath);
            
            var pathToFolder = _dialogues.GetBackupFolder(graph.Name);
            _csvExporter.Export(pathToFolder);
        }
    }
}