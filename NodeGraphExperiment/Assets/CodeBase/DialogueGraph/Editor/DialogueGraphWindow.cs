using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeBase.DialogueGraph.Editor
{
    public class DialogueGraphWindow : EditorWindow
    {
        private const string DialogueGraphName = "Dialogue Graph";
        
        private DialogueGraphView _graph;
        
        [MenuItem("Window/Dialogues")]
        public static void Open()
        {
            var window = GetWindow<DialogueGraphWindow>();
            window.titleContent = new GUIContent(DialogueGraphName);
        }

        private void OnEnable()
        {
            _graph = new DialogueGraphView
            {
                name = DialogueGraphName
            };
            
            _graph.Initialize();
            _graph.StretchToParentSize();
            rootVisualElement.Add(_graph);
        }

        private void OnDisable() =>
            rootVisualElement.Remove(_graph);
    }
}