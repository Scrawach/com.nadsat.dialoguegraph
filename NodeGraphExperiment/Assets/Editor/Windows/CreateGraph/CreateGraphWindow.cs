using System;
using System.IO;
using Editor.Data;
using Editor.Drawing.Controls;
using Editor.Extensions;
using Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Windows.CreateGraph
{
    public class CreateGraphWindow : BaseControl
    {
        private const string Uxml = "UXML/CreateGraphWindow";
        
        public new class UxmlFactory : UxmlFactory<CreateGraphWindow, UxmlTraits> { }

        private readonly DialoguesProvider _dialogues;
        
        private readonly TextField _nameField;
        private readonly TextField _locationField;
        private readonly Label _warningLabel;
        private readonly Button _createButton;
        private readonly Button _closeButton;

        private Action<DialogueGraphContainer> _onCreated;

        public CreateGraphWindow() : base(Uxml)
        {
            _nameField = this.Q<TextField>("name-field");
            _locationField = this.Q<TextField>("location-field");
            _createButton = this.Q<Button>("create-button");
            _warningLabel = this.Q<Label>("warning-label");
            _closeButton = this.Q<Button>("close-button");
            
            _nameField.RegisterValueChangedCallback(OnNameChanged);
            _createButton.clicked += OnCreateClicked;
            _closeButton.clicked += OnCloseClicked;

            _dialogues = new DialoguesProvider();
        }

        public event Action<DialogueGraph> Created;
        
        public event Action Closed
        {
            add => _closeButton.clicked += value;
            remove => _closeButton.clicked -= value;
        }

        public void Open(Action<DialogueGraphContainer> onCreated = null)
        {
            _onCreated = onCreated;
            this.Display(true);
        }

        private void OnCloseClicked() =>
            this.Display(false);

        private void OnNameChanged(ChangeEvent<string> evt) =>
            UpdateLocation(evt.newValue);

        private void OnCreateClicked()
        {
            var dialogueName = _nameField.value;
            
            if (string.IsNullOrWhiteSpace(dialogueName))
            {
                EditorUtility.DisplayDialog("Warning", "Enter name!", "OK");
                return;
            }

            if (_dialogues.Contains(dialogueName) && !OverrideWarningDialog())
                return;

            var container = _dialogues.CreateNewDialogue(dialogueName);
            _onCreated?.Invoke(container);
            Created?.Invoke(container.Graph);
            OnCloseClicked();
        }

        private static bool OverrideWarningDialog() =>
            EditorUtility.DisplayDialog("Warning", "Creating an asset with this name will result in overwriting an existing one. Are you sure?", "Yes, overwrite it", "No");

        private void UpdateLocation(string dialogueName)
        {
            _warningLabel.Display(_dialogues.Contains(dialogueName));
            _locationField.value = dialogueName;
        }
    }
}