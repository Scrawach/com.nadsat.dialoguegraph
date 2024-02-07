using System.IO;
using System.Linq;
using Editor.AssetManagement;
using Editor.Data;
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

        private LanguageProvider _languageProvider;
        private VariablesBlackboard _variablesBlackboard;
        private CreateGraphWindow _createWindow;
        private DialogueGraphRoot _graphRoot;

        private DialoguesProvider _dialogues;
        private PngExporter _pngExporter;

        public DialogueGraphToolbar() : base(Uxml)
        {
            _toolbarMenu = this.Q<ToolbarMenu>("assets-menu");
            _toolbarMenu.RegisterCallback<PointerEnterEvent>(OnPointerEnter);
            
            _languageDropdown = this.Q<DropdownField>("language-dropdown");
            _languageDropdown.RegisterValueChangedCallback(OnLanguageChanged);

            _variablesToggle = this.Q<Toggle>("variables-toggle");
            _variablesToggle.RegisterValueChangedCallback(OnVariablesToggled);

            _dialogues = new DialoguesProvider();
        }

        private void OnPointerEnter(PointerEnterEvent evt)
        {
            _toolbarMenu.menu.ClearItems();
            AppendMenuOptions(_toolbarMenu);
        }

        public void Initialize(VariablesBlackboard variablesBlackboard, LanguageProvider languageProvider, 
            PngExporter pngExporter, CreateGraphWindow createWindow, DialogueGraphRoot graphRoot)
        {
            _languageProvider = languageProvider;
            _variablesBlackboard = variablesBlackboard;
            _graphRoot = graphRoot;
            _createWindow = createWindow;
            _pngExporter = pngExporter;
            _languageProvider.Changed += () =>
            {
                _languageDropdown.value = _languageProvider.CurrentLanguage;
                _languageDropdown.choices = _languageProvider.AllLanguages().ToList();
            };
        }

        private void AppendMenuOptions(IToolbarMenuElement toolbar)
        {
            toolbar.menu.AppendAction("Create New...", (a) => { _createWindow.Open((graph) => _graphRoot.Populate(graph)); });
            toolbar.menu.AppendAction("Open/Open...", (a) => OpenAsset());
            AppendExistingDialogueGraphs(toolbar);

            toolbar.menu.AppendSeparator();
            toolbar.menu.AppendAction("Export/To Png", (a) => _pngExporter.Export());
        }

        private void AppendExistingDialogueGraphs(IToolbarMenuElement toolbar)
        {
            toolbar.menu.AppendSeparator("Open/");
            foreach (var graph in _dialogues.LoadAll())
                toolbar.menu.AppendAction($"Open/{graph.Graph.Name}", (a) => { _graphRoot.Populate(graph); });
        }

        private void OpenAsset()
        {
            var filePath = EditorUtility.OpenFilePanel("Dialogue Graph", _dialogues.GetRootPath(), "asset");
            if (string.IsNullOrWhiteSpace(filePath))
                return;

            var dialogueName = Path.GetFileNameWithoutExtension(filePath);
            var asset = _dialogues.Load(dialogueName);

            if (asset != null)
                _graphRoot.Populate(asset);
            else
                OpenInvalidAssetWarningWindow();
        }

        private bool OpenInvalidAssetWarningWindow() =>
            EditorUtility.DisplayDialog("Error", "You trying open invalid asset! You can load only DialogueGraph asset!", "OK");

        private void OnLanguageChanged(ChangeEvent<string> change) =>
            _languageProvider.ChangeLanguage(change.newValue);

        private void OnVariablesToggled(ChangeEvent<bool> evt)
        {
            if (evt.newValue)
                _variablesBlackboard.Show();
            else
                _variablesBlackboard.Hide();
        }
    }
}