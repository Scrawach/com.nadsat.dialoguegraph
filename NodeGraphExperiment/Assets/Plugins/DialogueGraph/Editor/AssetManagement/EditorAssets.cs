using UnityEngine;

namespace Nadsat.DialogueGraph.Editor.AssetManagement
{
    public class EditorAssets
    {
        public TAsset Load<TAsset>(string path) where TAsset : Object =>
            Resources.Load<TAsset>(path);
    }
}