using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace Editor.Serialization
{
    [Serializable]
    public class CopyPasteNodes
    {
        public List<GraphElement> ElementsToCopy = new List<GraphElement>();

        public void Add(GraphElement element) =>
            ElementsToCopy.Add(element);

        public void Clear() =>
            ElementsToCopy.Clear();
    }
}