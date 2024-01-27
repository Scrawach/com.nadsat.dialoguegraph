using System.Collections.Generic;
using System.Linq;
using Runtime;
using UnityEditor;

namespace Editor.Windows.Toolbar
{
    public class DialogueGraphAssets
    {
        private const string DialogueGraphContainer = "t:DialogueGraphContainer";

        public string[] GetExistingNames() =>
            LoadAll()
                .Select(container => container.Graph.Name)
                .ToArray();

        public IEnumerable<DialogueGraphContainer> LoadAll() =>
            AssetDatabase.FindAssets(DialogueGraphContainer)
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<DialogueGraphContainer>);
    }
}