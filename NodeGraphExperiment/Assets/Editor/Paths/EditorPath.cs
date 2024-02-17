using System.IO;
using UnityEditor;

namespace Editor.Paths
{
    public static class EditorPath
    {
        private const string DefaultResourcesPath = "Assets/Editor/Resources";
        private const string PackagesResourcesPath = "Packages/com.scrawach.DialogueGraph/Editor/Resources";
        
        public static string ToAbsolutePathUxml(string relativePath)
        {
            var result = Path.Combine(DefaultResourcesPath, relativePath);
            var resource = EditorGUIUtility.Load(result);

            if (resource == null) 
                result = Path.Combine(PackagesResourcesPath, relativePath);

            var withExtension = result + ".uxml";
            return withExtension;
        }
    }
}