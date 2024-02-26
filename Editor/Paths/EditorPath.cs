using System;
using System.IO;
using UnityEditor;

namespace Nadsat.DialogueGraph.Editor.Paths
{
    public static class EditorPath
    {
        private const string DefaultResourcesPath = "Assets/DialogueGraph/Editor/Resources";
        private const string PluginResourcesPath = "Assets/Plugins/DialogueGraph/Editor/Resources";
        private const string PackagesResourcesPath = "Packages/com.nadsat.dialoguegraph/Editor/Resources";
        
        public static string ToAbsolutePathUxml(string relativePath)
        {
            relativePath += ".uxml";

            if (IsResourceExistIn(DefaultResourcesPath, relativePath, out var path) || 
                IsResourceExistIn(PluginResourcesPath, relativePath, out path) ||
                IsResourceExistIn(PackagesResourcesPath, relativePath, out path))
                return path;

            throw new Exception($"Cannot find resource on path: {relativePath}");
        }

        private static bool IsResourceExistIn(string resourcesPath, string relativePath, out string path)
        {
            path = Path.Combine(resourcesPath, relativePath);
            return EditorGUIUtility.Load(path);
        }
    }
}