using System.Linq;
using Editor.AssetManagement;
using Editor.Drawing.Controls;
using Editor.Windows.Variables;
using UnityEngine.UIElements;

namespace Editor.Windows.Toolbar
{
    public class DialogueGraphToolbar : BaseControl
    {
        private const string Uxml = "UXML/DialogueGraphToolbar";
        
        public new class UxmlFactory : UxmlFactory<DialogueGraphToolbar, UxmlTraits> { }

        private readonly DropdownField _languageDropdown;
        private readonly Toggle _variablesToggle;
        
        private PhraseRepository _phraseRepository;
        private VariablesBlackboard _variablesBlackboard;
        
        public DialogueGraphToolbar() : base(Uxml)
        {
            _languageDropdown = this.Q<DropdownField>("language-dropdown");
            _languageDropdown.RegisterValueChangedCallback(OnLanguageChanged);

            _variablesToggle = this.Q<Toggle>("variables-toggle");
            _variablesToggle.RegisterValueChangedCallback(OnVariablesToggled);
        }

        public void Initialize(VariablesBlackboard variablesBlackboard, PhraseRepository phrases)
        {
            _phraseRepository = phrases;
            _variablesBlackboard = variablesBlackboard;
            _languageDropdown.value = phrases.CurrentLanguage;
            _languageDropdown.choices = phrases.AllLanguages().ToList();
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