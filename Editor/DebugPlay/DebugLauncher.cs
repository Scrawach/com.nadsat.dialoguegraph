using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nadsat.DialogueGraph.Editor.Data;
using Nadsat.DialogueGraph.Editor.Serialization;
using Nadsat.DialogueGraph.Editor.Serialization.Exporters;
using Nadsat.DialogueGraph.Runtime;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Nadsat.DialogueGraph.Editor.DebugPlay
{
    public class DebugLauncher
    {
        private readonly DialogueGraphExporter _exporter;
        private readonly DialogueGraphSerializer _serializer;
        private readonly DialoguesProvider _dialogues;

        public DebugLauncher(
            DialogueGraphExporter exporter,
            DialogueGraphSerializer serializer,
            DialoguesProvider dialogues)
        {
            _exporter = exporter;
            _serializer = serializer;
            _dialogues = dialogues;
        }

        public void LaunchCurrentDialogue()
        {
            _exporter.Export();
            var graph = _serializer.Serialize();
            
            Debug.LogError($"{graph.Nodes.Count}");
            var localization = GetLocalization(_dialogues.GetDialogueFolder(graph.Name)).ToList();

            if (localization.Count < 1)
            {
                Debug.LogError($"Not found localization for {graph.Name}");
                return;
            }
            
            const string testScenePath = "Assets/Scenes/Test/Dialogue Test Scene.unity";

            if (!AssetDatabase.AssetPathExists(testScenePath))
            {
                Debug.LogError($"Scene {testScenePath} not found!");
                return;
            }

            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) 
                return;

            EditorSceneManager.OpenScene(testScenePath);
            var dialogueScene = Object.FindAnyObjectByType<DebugDialogueScene>();
            dialogueScene.Graph = graph;
            dialogueScene.Localization = localization;
            EditorApplication.isPlaying = true;
        }

        private IEnumerable<TextAsset> GetLocalization(string path) => 
            GetCsvFilesFromDirectory(path).Select(AssetDatabase.LoadAssetAtPath<TextAsset>);

        private IEnumerable<string> GetCsvFilesFromDirectory(string path)
        {
            const string tableExtensions = ".csv";
            var filesInDirectory = Directory.GetFiles(path);
            foreach (var filePath in filesInDirectory)
            {
                if (Path.GetExtension(filePath) == tableExtensions)
                    yield return filePath;
            }
        }
    }
}