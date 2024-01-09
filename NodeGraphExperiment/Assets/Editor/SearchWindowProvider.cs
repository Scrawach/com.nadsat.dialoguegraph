using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Editor
{
    public class SearchWindowProvider
    {
        private readonly UnityEditor.EditorWindow _owner;
        
        private StringSearchWindow _searchWindow;

        public SearchWindowProvider(UnityEditor.EditorWindow owner) =>
            _owner = owner;

        public void FindKeys(Vector2 position, Action<string> onSelected = null)
        {
            var point = _owner.position.position + position + new Vector2(150, 0);
            if (_searchWindow == null)
                _searchWindow = ScriptableObject.CreateInstance<StringSearchWindow>();
            _searchWindow.Configure("Keys", TestKeys(), onSelected);
            SearchWindow.Open(new SearchWindowContext(point), _searchWindow);
        }

        private string[] TestKeys()
        {
            var result = new List<string>();
            for (var i = 0; i < 100; i++) 
                result.Add($"tutor_elena_{i}");
            return result.ToArray();
        }
    }
}