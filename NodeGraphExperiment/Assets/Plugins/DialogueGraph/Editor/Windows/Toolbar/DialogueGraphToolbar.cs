using System.Linq;
using Editor.AssetManagement;
using Editor.Drawing.Controls;
using Editor.Windows.Variables;
using UnityEngine.UIElements;

namespace Editor.Windows.Toolbar
{
    public class DialogueGraphToolbar : BaseControl
    {
        private const string Uxml = "UXML/Toolbars/DialogueGraphToolbar";

        private readonly DropdownField _languageDropdown;
        private readonly Toggle _variablesToggle;

        private LanguageProvider _languageProvider;
        private VariablesBlackboard _variablesBlackboard;

        public DialogueGraphToolbar() : base(Uxml)
        {
            _languageDropdown = this.Q<DropdownField>("language-dropdown");
            _languageDropdown.RegisterValueChangedCallback(OnLanguageChanged);

            _variablesToggle = this.Q<Toggle>("variables-toggle");
            _variablesToggle.RegisterValueChangedCallback(OnVariablesToggled);
        }

        public void Initialize(VariablesBlackboard variablesBlackboard, LanguageProvider languageProvider)
        {
            _languageProvider = languageProvider;
            _variablesBlackboard = variablesBlackboard;

            _languageProvider.Changed += () =>
            {
                _languageDropdown.value = _languageProvider.CurrentLanguage;
                _languageDropdown.choices = _languageProvider.AllLanguages().ToList();
            };
        }

        private void OnLanguageChanged(ChangeEvent<string> change) =>
            _languageProvider.ChangeLanguage(change.newValue);

        private void OnVariablesToggled(ChangeEvent<bool> evt)
        {
            if (evt.newValue)
                _variablesBlackboard.Show();
            else
                _variablesBlackboard.Hide();
        }

        public new class UxmlFactory : UxmlFactory<DialogueGraphToolbar, UxmlTraits> { }
    }
}