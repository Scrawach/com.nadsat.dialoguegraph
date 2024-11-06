using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nadsat.DialogueGraph.Runtime;
using UnityEditor;
using UnityEngine;

namespace Nadsat.DialogueGraph.Editor.Data
{
    public class DialoguesProvider
    {
        private const string DialogueGraphContainer = "t:DialogueGraphContainer";
        
        private string _rootFolder = "Assets/Resources/Dialogues/";

        public void SetupRootFolder(string path) => 
            _rootFolder = path;

        public string GetRootPath() =>
            _rootFolder;

        public string GetDialoguePath(string dialogueName) =>
            $"{GetRootPath()}/{dialogueName}.asset";

        public string GetDialogueFolder(string dialogueName) =>
            $"{_rootFolder}";

        public string GetBackupDialoguePath(string dialogueName) =>
            $"{GetBackupFolder(dialogueName)}/{dialogueName}.asset";

        public string GetBackupFolder(string dialogueName) =>
            $"{_rootFolder}/Backup";
        
        public bool Contains(string dialogueName) =>
            AssetDatabase.AssetPathExists(GetDialoguePath(dialogueName));

        public DialogueGraphContainer CreateNewDialogue(string dialogueName)
        {
            var pathToDialogueAsset = GetDialoguePath(dialogueName);
            CreateDirectoriesForFile(pathToDialogueAsset);
            var container = ScriptableObject.CreateInstance<DialogueGraphContainer>();
            var graph = new Runtime.DialogueGraph {Name = dialogueName};
            container.Graph = graph;
            AssetDatabase.CreateAsset(container, pathToDialogueAsset);
            AssetDatabase.SaveAssets();
            return container;
        }

        public DialogueGraphContainer CreateNewDialogue(Runtime.DialogueGraph graph, string path)
        {
            CreateDirectoriesForFile(path);
            var asset = ScriptableObject.CreateInstance<DialogueGraphContainer>();
            asset.Graph = graph;
            var clone = Object.Instantiate(asset);
            AssetDatabase.CreateAsset(clone, path);
            AssetDatabase.SaveAssetIfDirty(clone);
            return clone;
        }

        public string[] GetExistingNames() =>
            LoadAll()
                .Select(container => container.Graph.Name)
                .ToArray();

        public DialogueGraphContainer Load(string dialogueName)
        {
            var pathToDialogueAsset = GetDialoguePath(dialogueName);
            return AssetDatabase.LoadAssetAtPath<DialogueGraphContainer>(pathToDialogueAsset);
        }

        public IEnumerable<DialogueGraphContainer> LoadAll() =>
            AssetDatabase.FindAssets(DialogueGraphContainer)
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<DialogueGraphContainer>);

        private static void CreateDirectoriesForFile(string path)
        {
            var relativePath = Path.GetRelativePath("Assets", path);
            var targetDirectoryPath = Path.GetDirectoryName(relativePath);
            var fullPathToDirectory = Path.Combine(Application.dataPath, targetDirectoryPath);

            if (!Directory.Exists(fullPathToDirectory))
                Directory.CreateDirectory(fullPathToDirectory);
        }
    }
}