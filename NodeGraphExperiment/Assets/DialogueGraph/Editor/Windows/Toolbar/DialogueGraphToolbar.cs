using System.Linq;
using DialogueGraph.Editor.AssetManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueGraph.Editor.Windows.Toolbar
{
    public class DialogueGraphToolbar : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<DialogueGraphToolbar, UxmlTraits> { }

        private readonly DropdownField _languageDropdown;
        private PhraseRepository _phraseRepository;
        
        public DialogueGraphToolbar()
        {
            var uxml = Resources.Load<VisualTreeAsset>("UXML/DialogueGraphToolbar");
            uxml.CloneTree(this);
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