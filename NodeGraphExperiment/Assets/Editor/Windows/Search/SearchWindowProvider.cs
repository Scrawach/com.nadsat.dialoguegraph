using System;
using Editor.AssetManagement;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Editor.Windows.Search
{
    public class SearchWindowProvider
    {
        private readonly UnityEditor.EditorWindow _owner;
        private readonly PhraseRepository _phraseRepository;

        private StringSearchWindow _searchWindow;

        public SearchWindowProvider(UnityEditor.EditorWindow owner, PhraseRepository phraseRepository)
        {
            _owner = owner;
            _phraseRepository = phraseRepository;
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