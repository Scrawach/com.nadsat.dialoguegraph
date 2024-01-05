using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class DialogueGraphWindow : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;
        
        [MenuItem("Window/Dialogue Graph")]
        public static void OpenWindow()
        {
            var window = GetWindow<DialogueGraphWindow>();
            window.titleContent = new GUIContent("DialogueGraph");
        }

        public void CreateGUI()
        {
            var root = rootVisualElement;
            VisualElement tree = m_VisualTreeAsset.Instantiate();
            
            tree.StretchToParentSize();
            tree.Q("LeftPanel").style.display = DisplayStyle.None;
            root.Add(tree);
        }
    }
}
