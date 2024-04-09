using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nadsat.DialogueGraph.Runtime
{
    [Serializable]
    public class PersonData
    {
        public string Name;
        public Sprite Icon;
        public Color Color;
        public List<PersonEmotion> Emotions;
    }
}