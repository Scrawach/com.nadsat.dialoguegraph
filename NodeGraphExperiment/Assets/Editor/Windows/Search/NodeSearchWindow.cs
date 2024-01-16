using System;
using System.Collections.Generic;
using Editor.Drawing.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Editor.Windows.Search
{
    public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private string _title;
        private DialogueNodeView[] _nodeViews;
        private string[] _tooltips;
        private Action<DialogueNodeView> _onSelected;

        public void Configure(string title, DialogueNodeView[] nodes, string[] tooltips, Action<DialogueNodeView> onSelected = null)
        {
            _title = title;
            _nodeViews = nodes;
            _onSelected = onSelected;
            _tooltips = tooltips;
        }
        
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
            if (entry.userData is not DialogueNodeView node) 
                return false;
            
            _onSelected?.Invoke(node);
            return true;
        }
    }
}