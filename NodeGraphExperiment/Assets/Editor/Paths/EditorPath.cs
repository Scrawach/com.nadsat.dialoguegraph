using System.IO;

namespace Editor.Paths
{
    public static class EditorPath
    {
        public static string ToAbsolutePathUxml(string relativePath)
        {
            var result = Path.Combine("Assets/Editor/Resources", relativePath);
            var resultWithExtension = result + ".uxml";
            return resultWithExtension;
        }
    }
}