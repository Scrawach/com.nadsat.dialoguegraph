using Editor.Data;
using Editor.Serialization;
using Runtime;
using UnityEditor;
using UnityEngine;

namespace Editor.Exporters
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
            ExportDialogueContainer(graph);
            ExportCsvContent(graph);
        }

        private void ExportDialogueContainer(DialogueGraph graph)
        {
            var asset = ScriptableObject.CreateInstance<DialogueGraphContainer>();
            asset.Graph = graph;
            
            var assetPath = _dialogues.GetDialoguePath(graph.Name);
            var clone = Object.Instantiate(asset);
            AssetDatabase.CreateAsset(clone, assetPath);
            AssetDatabase.SaveAssetIfDirty(clone);
            EditorGUIUtility.PingObject(clone);
        }

        private void ExportCsvContent(DialogueGraph graph)
        {
            var pathToFolder = _dialogues.GetDialogueFolder(graph.Name);
            _csvExporter.Export(pathToFolder);
        }
    }
}