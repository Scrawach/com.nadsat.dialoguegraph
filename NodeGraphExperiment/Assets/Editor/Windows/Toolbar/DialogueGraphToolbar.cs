using System.IO;
using System.Linq;
using Editor.AssetManagement;
using Editor.Drawing;
using Editor.Drawing.Controls;
using Editor.Exporters;
using Editor.Windows.CreateGraph;
using Editor.Windows.Variables;
using Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Windows.Toolbar
{
    public class DialogueGraphToolbar : BaseControl
    {
        private const string Uxml = "UXML/DialogueGraphToolbar";
        
        public new class UxmlFactory : UxmlFactory<DialogueGraphToolbar, UxmlTraits> { }

        private readonly ToolbarMenu _toolbarMenu;
        private readonly DropdownField _languageDropdown;
        private readonly Toggle _variablesToggle;
        
        private PhraseRepository _phraseRepository;
        private VariablesBlackboard _variablesBlackboard;
        private EditorWindow _root;
        private DialogueGraphView _graphView;
        private CreateGraphWindow _createWindow;

        public DialogueGraphToolbar() : base(Uxml)
        {
            _toolbarMenu = this.Q<ToolbarMenu>("assets-menu");
            _toolbarMenu.RegisterCallback<PointerEnterEvent>(OnPointerEnter);
            
            _languageDropdown = this.Q<DropdownField>("language-dropdown");
            _languageDropdown.RegisterValueChangedCallback(OnLanguageChanged);

            _variablesToggle = this.Q<Toggle>("variables-toggle");
            _variablesToggle.RegisterValueChangedCallback(OnVariablesToggled);
        }

        private void OnPointerEnter(PointerEnterEvent evt)
        {
            _toolbarMenu.menu.ClearItems();
            AppendMenuOptions(_toolbarMenu);
        }

        private void OnClicked(ClickEvent evt)
        {
            Debug.Log("CLICKED!");
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            Debug.Log($"POINTER DOWN!");
        }

        public void Initialize(VariablesBlackboard variablesBlackboard, PhraseRepository phrases, EditorWindow root, DialogueGraphView graphView, CreateGraphWindow createWindow)
        {
            _phraseRepository = phrases;
            _variablesBlackboard = variablesBlackboard;
            _root = root;
            _graphView = graphView;
            _createWindow = createWindow;
            _languageDropdown.value = phrases.CurrentLanguage;
            _languageDropdown.choices = phrases.AllLanguages().ToList();
        }

        private void AppendMenuOptions(IToolbarMenuElement toolbar)
        {
            toolbar.menu.AppendAction("Create New...", (a) => { _createWindow.Open((graph) => _graphView.Populate(graph)); });
            toolbar.menu.AppendAction("Open/Open...", (a) => OpenAsset());
            AppendExistingDialogueGraphs(toolbar);

            toolbar.menu.AppendSeparator();
            toolbar.menu.AppendAction("Export/To Png", (a) =>
            {
                var exporter = new PngExporter(_root, _graphView);
                exporter.Export();
            });
        }

        private void AppendExistingDialogueGraphs(IToolbarMenuElement toolbar)
        {
            var dialogueGraphAssets = new DialogueGraphAssets();
            toolbar.menu.AppendSeparator("Open/");
            foreach (var graph in dialogueGraphAssets.LoadAll())
                toolbar.menu.AppendAction($"Open/{graph.Graph.Name}", (a) => { _graphView.Populate(graph); });
        }

        private void OpenAsset()
        {
            var filePath = EditorUtility.OpenFilePanel("Dialogue Graph", "Assets/Resources/Dialogues", "asset");

            if (string.IsNullOrWhiteSpace(filePath))
                return;
            
            var fromAssetPath = filePath.Split("Assets/");
            var pathToAsset = Path.Combine("Assets", fromAssetPath[1]);
            var asset = AssetDatabase.LoadAssetAtPath<DialogueGraphContainer>(pathToAsset);

            if (asset != null)
                _graphView.Populate(asset);
            else
                OpenInvalidAssetWarningWindow();
        }

        private bool OpenInvalidAssetWarningWindow() =>
            EditorUtility.DisplayDialog("Error", "You trying open invalid asset! You can load only DialogueGraph asset!", "OK");

        private void OnLanguageChanged(ChangeEvent<string> change) =>
            _phraseRepository.ChangeLanguage(change.newValue);

        private void OnVariablesToggled(ChangeEvent<bool> evt)
        {
            if (evt.newValue)
                _variablesBlackboard.Show();
            else
                _variablesBlackboard.Hide();
        }
    }
}