using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Nadsat.DialogueGraph.Editor.Windows.Search
{
    public class StringSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private string[] _choices;
        private Action<string> _onSelected;
        private string _title;
        private string[] _tooltips;

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>();
            tree.Add(new SearchTreeGroupEntry(new GUIContent(_title)));
            for (var i = 0; i < _choices.Length; i++)
            {
                var content = new GUIContent($"[{_choices[i]}] {_tooltips[i]}");
                tree.Add(new SearchTreeEntry(content) {level = 1, userData = _choices[i]});
            }

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
        {
            if (entry.userData is not string line)
                return false;

            _onSelected?.Invoke(line);
            return true;
        }

        public void Configure(string title, string[] choices, string[] tooltips, Action<string> onSelected = null)
        {
            _title = title;
            _choices = choices;
            _tooltips = tooltips;
            _onSelected = onSelected;
        }
    }
}