using System;
using UnityEditor;
using UnityEngine;

namespace Runtime
{
    [Serializable]
    public class PersonData
    {
        public string Name;
        public Sprite Icon;
        public Color Color;

        public string PathToIcon =>
            Icon == null
                ? string.Empty
                : AssetDatabase.GetAssetPath(Icon);
    }
}