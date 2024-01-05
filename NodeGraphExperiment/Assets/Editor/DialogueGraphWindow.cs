using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class DialogueGraphWindow : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;
        
        [MenuItem("Dialogue Graph/Open")]
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
            root.Add(tree);
        }
    }
}
