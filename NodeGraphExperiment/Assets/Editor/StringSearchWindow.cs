using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Editor
{
    public class StringSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private string _title;
        private string[] _choices;
        private Action<string> _onSelected;

        public void Configure(string title, string[] choices, Action<string> onSelected = null)
        {
            _title = title;
            _choices = choices;
            _onSelected = onSelected;
        }
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>();
            tree.Add(new SearchTreeGroupEntry(new GUIContent(_title)));
            foreach (var key in _choices) 
                tree.Add(new SearchTreeEntry(new GUIContent(key)) {level = 1, userData = key});
            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
        {
            if (entry.userData is not string line) 
                return false;
            
            _onSelected?.Invoke(line);
            return true;
        }
    }
}