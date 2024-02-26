using System;
using System.Collections.Generic;
using System.Linq;
using Nadsat.DialogueGraph.Editor.AssetManagement;
using Nadsat.DialogueGraph.Editor.Audios;
using Nadsat.DialogueGraph.Editor.Drawing;
using Nadsat.DialogueGraph.Editor.Drawing.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Nadsat.DialogueGraph.Editor.Windows.Search
{
    public class SearchWindowProvider
    {
        private readonly ChoicesRepository _choices;
        private readonly IAudioEventsProvider _audioEventsProvider;
        private readonly EditorWindow _owner;
        private readonly PhraseRepository _phraseRepository;
        private readonly DialogueGraphView _view;
        private NodeSearchWindow _nodeSearchWindow;

        private StringSearchWindow _searchWindow;

        public SearchWindowProvider(EditorWindow owner, DialogueGraphView view, PhraseRepository phraseRepository, 
            ChoicesRepository choices, IAudioEventsProvider audioEventsProvider)
        {
            _owner = owner;
            _view = view;
            _phraseRepository = phraseRepository;
            _choices = choices;
            _audioEventsProvider = audioEventsProvider;
        }

        public void FindNodes(Vector2 position, Action<Node> onSelected = null)
        {
            const int searchWindowWidth = 600;
            var point = _owner.position.position + position + new Vector2(searchWindowWidth / 3f, 0);

            if (_nodeSearchWindow == null)
                _nodeSearchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();

            var (nodes, tooltips) = BuildNodes();
            _nodeSearchWindow.Configure("Nodes", nodes, tooltips, onSelected);
            SearchWindow.Open(new SearchWindowContext(point, searchWindowWidth), _nodeSearchWindow);
        }

        private (Node[] nodes, string[] tooltips) BuildNodes()
        {
            var nodes = new List<Node>();
            var tooltips = new List<string>();

            foreach (var node in _view.nodes)
                if (node is DialogueNodeView nodeView)
                {
                    var model = nodeView.Model;
                    tooltips.Add(string.IsNullOrEmpty(model.PhraseId)
                        ? $"[{model.PersonId}] none [{model.Guid}]"
                        : $"[{model.PersonId}] {_phraseRepository.Get(model.PhraseId)} [{model.Guid}]");

                    nodes.Add(nodeView);
                }
                else if (node is ChoicesNodeView choiceView)
                {
                    var model = choiceView.Model;
                    foreach (var choice in model.Choices)
                    {
                        tooltips.Add($"[{choice}] {_choices.Get(choice)}");
                        nodes.Add(choiceView);
                    }
                }

            return (nodes.ToArray(), tooltips.ToArray());
        }

        public void FindEvents(Vector2 position, Action<string> onSelected = null)
        {
            const int searchWindowWidth = 600;
            var point = _owner.position.position + position + new Vector2(searchWindowWidth / 3f, 0);
            
            if (_searchWindow == null)
                _searchWindow = ScriptableObject.CreateInstance<StringSearchWindow>();

            var (choices, tooltips) = BuildEvents();
            _searchWindow.Configure("Audio Event", choices, tooltips, onSelected);
            SearchWindow.Open(new SearchWindowContext(point, searchWindowWidth), _searchWindow);
        }

        private (string[] choices, string[] tooltips) BuildEvents()
        {
            var choices = _audioEventsProvider.AllEvents().ToArray();
            var tooltips = new List<string>();
            
            for (var i = 0; i < choices.Length; i++) 
                tooltips.Add(string.Empty);

            return (choices, tooltips.ToArray());
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

        private (string[] choices, string[] tooltips) BuildPhrases() =>
            (null, null);
        //var keys = _phraseRepository.AllKeys();
        //var tooltips = new string[keys.Length];
        //for (var i = 0; i < tooltips.Length; i++)
        //{
        //    tooltips[i] = _phraseRepository.Get(keys[i]);
        //}
        //return (keys, tooltips);
    }
}