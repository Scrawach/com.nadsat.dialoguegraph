using System;
using System.Collections.Generic;
using System.Linq;
using Editor.AssetManagement;
using Editor.Drawing.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Editor.Windows.Search
{
    public class SearchWindowProvider
    {
        private readonly UnityEditor.EditorWindow _owner;
        private readonly DialogueGraphView _view;
        private readonly PhraseRepository _phraseRepository;

        private StringSearchWindow _searchWindow;
        private NodeSearchWindow _nodeSearchWindow;

        public SearchWindowProvider(UnityEditor.EditorWindow owner, DialogueGraphView view, PhraseRepository phraseRepository)
        {
            _owner = owner;
            _view = view;
            _phraseRepository = phraseRepository;
        }

        public void FindNodes(Vector2 position, Action<DialogueNodeView> onSelected = null)
        {
            const int searchWindowWidth = 600;
            var point = _owner.position.position + position + new Vector2(searchWindowWidth / 3f, 0);
            
            if (_nodeSearchWindow == null)
                _nodeSearchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();

            var (nodes, tooltips) = BuildNodes();
            _nodeSearchWindow.Configure("Nodes", nodes, tooltips, onSelected);
            SearchWindow.Open(new SearchWindowContext(point, searchWindowWidth), _nodeSearchWindow);
        }
        
        private (DialogueNodeView[] nodes, string[] tooltips) BuildNodes()
        {
            var nodes = new List<DialogueNodeView>();
            var tooltips = new List<string>();

            foreach (var node in _view.nodes)
            {
                if (node is DialogueNodeView nodeView)
                {
                    var model = nodeView.Model;
                    if (string.IsNullOrWhiteSpace(model.PhraseId))
                        tooltips.Add($"[{model.PersonId}] none [{model.Guid}]");
                    else
                        tooltips.Add($"[{model.PersonId}] {_phraseRepository.Get(model.PhraseId)} [{model.Guid}]");
                    nodes.Add(nodeView);
                }
            }

            return (nodes.ToArray(), tooltips.ToArray());
        }
        
        public void FindPhrase(Vector2 position, Action<string> onSelected = null)
        {
            const int searchWindowWidth = 600;
            var point = _owner.position.position + position + new Vector2(searchWindowWidth / 3f, 0);
            
            if (_searchWindow == null)
                _searchWindow = ScriptableObject.CreateInstance<StringSearchWindow>();

            var (choices, tooltips) = BuildPhrases();
            _searchWindow.Configure("Phrases", choices, tooltips, onSelected);
            SearchWindow.Open(new SearchWindowContext(point, searchWindowWidth), _searchWindow);
        }
        
        private (string[] choices, string[] tooltips) BuildPhrases()
        {
            var keys = _phraseRepository.AllKeys();
            var tooltips = new string[keys.Length];
            for (var i = 0; i < tooltips.Length; i++)
            {
                tooltips[i] = _phraseRepository.Get(keys[i]);
            }

            return (keys, tooltips);
        }
    }
}