using System.Linq;
using Editor.AssetManagement;
using Editor.Drawing.Controls;
using Editor.Exporters;
using Editor.Extensions;
using Editor.Windows.CreateGraph;
using Editor.Windows.Variables;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
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
        private GraphView _graphView;
        private CreateGraphWindow _createWindow;

        public DialogueGraphToolbar() : base(Uxml)
        {
            _toolbarMenu = this.Q<ToolbarMenu>("assets-menu");
            AppendMenuOptions(_toolbarMenu);
            
            _languageDropdown = this.Q<DropdownField>("language-dropdown");
            _languageDropdown.RegisterValueChangedCallback(OnLanguageChanged);

            _variablesToggle = this.Q<Toggle>("variables-toggle");
            _variablesToggle.RegisterValueChangedCallback(OnVariablesToggled);
        }

        public void Initialize(VariablesBlackboard variablesBlackboard, PhraseRepository phrases, EditorWindow root, GraphView graphView, CreateGraphWindow createWindow)
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
            toolbar.menu.AppendAction("Create New...", (a) => { _createWindow.Display(true); });
            toolbar.menu.AppendSeparator();
            toolbar.menu.AppendAction("Export/To Png", (a) =>
            {
                var exporter = new PngExporter(_root, _graphView);
                exporter.Export();
            });
        }

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