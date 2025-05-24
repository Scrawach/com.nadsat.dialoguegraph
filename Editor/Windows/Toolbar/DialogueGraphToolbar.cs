using System;
using System.IO;
using System.Linq;
using Nadsat.DialogueGraph.Editor.AssetManagement;
using Nadsat.DialogueGraph.Editor.DebugPlay;
using Nadsat.DialogueGraph.Editor.Drawing.Controls;
using Nadsat.DialogueGraph.Editor.Windows.Variables;
using Nadsat.DialogueGraph.Runtime;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace Nadsat.DialogueGraph.Editor.Windows.Toolbar
{
    public class DialogueGraphToolbar : BaseControl
    {
        private const string Uxml = "UXML/Toolbars/DialogueGraphToolbar";

        private readonly DropdownField _languageDropdown;
        private readonly Toggle _variablesToggle;
        private readonly Button _playButton;

        private LanguageProvider _languageProvider;
        private VariablesBlackboard _variablesBlackboard;
        private DebugLauncher _debugLauncher;

        public DialogueGraphToolbar() : base(Uxml)
        {
            _languageDropdown = this.Q<DropdownField>("language-dropdown");
            _languageDropdown.RegisterValueChangedCallback(OnLanguageChanged);

            _variablesToggle = this.Q<Toggle>("variables-toggle");
            _variablesToggle.RegisterValueChangedCallback(OnVariablesToggled);

            _playButton = this.Q<Button>("play-button");
            _playButton.RegisterCallback<ClickEvent>(PlayCurrentDialogue);;
        }
        
        public void Initialize(
            VariablesBlackboard variablesBlackboard, 
            LanguageProvider languageProvider,
            DebugLauncher debugLauncher)
        {
            _languageProvider = languageProvider;
            _variablesBlackboard = variablesBlackboard;
            _debugLauncher = debugLauncher;

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
        
        private void PlayCurrentDialogue(ClickEvent click) => 
            _debugLauncher.LaunchCurrentDialogue();

        public new class UxmlFactory : UxmlFactory<DialogueGraphToolbar, UxmlTraits> { }
    }
}