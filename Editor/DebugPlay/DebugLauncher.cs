using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nadsat.DialogueGraph.Editor.Data;
using Nadsat.DialogueGraph.Editor.Serialization.Exporters;
using Nadsat.DialogueGraph.Runtime;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Nadsat.DialogueGraph.Editor.DebugPlay
{
    public class DebugLauncher
    {
        private readonly DialogueGraphExporter _graphExporter;
        private readonly DialogueGraphProvider _graphProvider;
        private readonly DialoguesProvider _dialogues;

        public DebugLauncher(
            DialogueGraphExporter graphExporter, 
            DialogueGraphProvider graphProvider,
            DialoguesProvider dialogues)
        {
            _graphExporter = graphExporter;
            _graphProvider = graphProvider;
            _dialogues = dialogues;
        }

        public void LaunchCurrentDialogue()
        {
            _graphExporter.Export();
            
            Debug.LogError($"{_graphProvider.Graph}");
            var localization = GetLocalization(_dialogues.GetDialogueFolder(_graphProvider.Graph.Name)).ToList();

            if (localization.Count < 1)
            {
                Debug.LogError($"Not found localization for {_graphProvider.Graph}");
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
            dialogueScene.Graph = _graphProvider.Graph;
            dialogueScene.Localization = localization;
            EditorApplication.isPlaying = true;
        }

        private IEnumerable<TextAsset> GetLocalization(string path) => 
            GetCsvFilesFromDirectory(path).Select(AssetDatabase.LoadAssetAtPath<TextAsset>);

        private IEnumerable<string> GetCsvFilesFromDirectory(string path)
        {
            Debug.LogError($"START: {path}");
            
            const string tableExtensions = ".csv";
            var filesInDirectory = Directory.GetFiles(path);
            foreach (var filePath in filesInDirectory)
            {
                Debug.LogError($"File path: {filePath}");
                
                if (Path.GetExtension(filePath) == tableExtensions)
                    yield return filePath;
            }
        }
    }
}