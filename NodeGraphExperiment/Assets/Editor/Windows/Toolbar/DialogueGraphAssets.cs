using System.Collections.Generic;
using System.Linq;
using Runtime;
using UnityEditor;

namespace Editor.Windows.Toolbar
{
    public class DialogueGraphAssets
    {
        public string[] GetExistingNames()
        {
            return null;
        }

        public IEnumerable<DialogueGraphContainer> LoadAll() =>
            AssetDatabase.FindAssets("t:DialogueGraph")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<DialogueGraphContainer>);
    }
}