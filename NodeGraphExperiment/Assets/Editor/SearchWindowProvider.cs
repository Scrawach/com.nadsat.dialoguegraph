using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Editor
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

        public void FindKeys(Vector2 position, Action<string> onSelected = null)
        {
            var point = _owner.position.position + position + new Vector2(150, 0);
            if (_searchWindow == null)
                _searchWindow = ScriptableObject.CreateInstance<StringSearchWindow>();
            _searchWindow.Configure("Keys", _phraseRepository.AllKeys(), onSelected);
            SearchWindow.Open(new SearchWindowContext(point), _searchWindow);
        }
    }
}