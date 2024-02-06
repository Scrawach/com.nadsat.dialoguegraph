using System.IO;
using Runtime;
using UnityEditor;
using UnityEngine;

namespace Editor.Data
{
    public class DialoguesProvider
    {
        private const string RootFolder = "Assets/Resources/Dialogues/";
        
        public string GetRootPath() =>
            RootFolder;

        public string GetDialoguePath(string dialogueName) =>
            $"{RootFolder}{dialogueName}/{dialogueName}.asset";
        
        public bool Contains(string dialogueName) =>
            AssetDatabase.AssetPathExists(GetDialoguePath(dialogueName));

        public DialogueGraphContainer CreateNewDialogue(string dialogueName)
        {
            var pathToDialogueAsset = GetDialoguePath(dialogueName);
            CreateDirectoriesForFile(pathToDialogueAsset);
            var container = ScriptableObject.CreateInstance<DialogueGraphContainer>();
            var graph = new DialogueGraph { Name = dialogueName };
            container.Graph = graph;
            AssetDatabase.CreateAsset(container, pathToDialogueAsset);
            AssetDatabase.SaveAssets();
            EditorGUIUtility.PingObject(container);
            return container;
        }
        
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