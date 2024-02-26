using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Nadsat.DialogueGraph.Editor.Windows.Search
{
    public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private Node[] _nodeViews;
        private Action<Node> _onSelected;
        private string _title;
        private string[] _tooltips;

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent(_title))
            };

            for (var i = 0; i < _nodeViews.Length; i++)
            {
                var content = new GUIContent($"{_tooltips[i]}");
                tree.Add(new SearchTreeEntry(content) {level = 1, userData = _nodeViews[i]});
            }

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
        {
            if (entry.userData is not Node node)
                return false;

            _onSelected?.Invoke(node);
            return true;
        }

        public void Configure(string title, Node[] nodes, string[] tooltips, Action<Node> onSelected = null)
        {
            _title = title;
            _nodeViews = nodes;
            _onSelected = onSelected;
            _tooltips = tooltips;
        }
    }
}