using System.Linq;
using Editor.AssetManagement;
using Editor.Drawing.Controls;
using UnityEngine.UIElements;

namespace Editor.Windows.Toolbar
{
    public class DialogueGraphToolbar : BaseControl
    {
        private const string Uxml = "UXML/DialogueGraphToolbar";
        
        public new class UxmlFactory : UxmlFactory<DialogueGraphToolbar, UxmlTraits> { }

        private readonly DropdownField _languageDropdown;
        private PhraseRepository _phraseRepository;
        
        public DialogueGraphToolbar() : base(Uxml)
        {
            _languageDropdown = this.Q<DropdownField>("language-dropdown");
            _languageDropdown.RegisterValueChangedCallback(OnLanguageChanged);
        }

        public void Initialize(PhraseRepository phrases)
        {
            _phraseRepository = phrases;
            _languageDropdown.value = phrases.CurrentLanguage;
            _languageDropdown.choices = phrases.AllLanguages().ToList();
        }

        private void OnLanguageChanged(ChangeEvent<string> change) =>
            _phraseRepository.ChangeLanguage(change.newValue);
    }
}