using UnityEngine;

namespace DialogueGraph.Editor.AssetManagement
{
    public class EditorAssets
    {
        public TAsset Load<TAsset>(string path) where TAsset : UnityEngine.Object =>
            Resources.Load<TAsset>(path);
    }
}