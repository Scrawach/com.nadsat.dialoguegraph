using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class DialogueGraphWindow : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;

        private DialogueGraphView _dialogueGraphView;
        private InspectorView _inspectorView;
        
        [MenuItem("Dialogue Graph/Open")]
        public static void OpenWindow()
        {
            var window = GetWindow<DialogueGraphWindow>();
            window.titleContent = new GUIContent(nameof(DialogueGraphWindow));
        }

        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            /*if (Selection.activeObject is DialogueGraph)
            {
                OpenWindow();
                return true;
            }*/

            return false;
        }
        
        public void CreateGUI()
        {
            var root = rootVisualElement;
            VisualElement tree = m_VisualTreeAsset.Instantiate();
            
            tree.StretchToParentSize();
            root.Add(tree);

            _dialogueGraphView = root.Q<DialogueGraphView>();
            _inspectorView = root.Q<InspectorView>();

            var personDatabase = AssetDatabase.LoadAssetAtPath<DialoguePersonDatabase>("Assets/Editor/Dialogue Person Database.asset");
            var inspectorFactory = new InspectorViewFactory(personDatabase);
            _dialogueGraphView.Initialize(personDatabase);
            _dialogueGraphView.OnNodeSelected += (node) => _inspectorView.Populate(inspectorFactory.Build(node));
            _dialogueGraphView.graphViewChanged += OnChange;
        }

        private GraphViewChange OnChange(GraphViewChange graphViewChange)
        {
            hasUnsavedChanges = true;
            return graphViewChange;
        }

        public override void SaveChanges()
        {
            base.SaveChanges();
            // Save data to asset or something else idk
        }
    }
}
