using System;
using System.IO;
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

        private readonly TextField _nameField;
        private readonly TextField _locationField;
        private readonly Label _warningLabel;
        private readonly Button _createButton;
        private readonly Button _closeButton;

        private Action<DialogueGraph> _onCreated;
        
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
        }

        public event Action<DialogueGraph> Created;
        
        public event Action Closed
        {
            add => _closeButton.clicked += value;
            remove => _closeButton.clicked -= value;
        }

        public void Open(Action<DialogueGraph> onCreated = null)
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
            if (string.IsNullOrWhiteSpace(_nameField.value))
            {
                EditorUtility.DisplayDialog("Warning", "Enter name!", "OK");
                return;
            }
            
            var canWrite = true;
            
            if (ThisAssetAlreadyExisting(_locationField.value)) 
                canWrite = WarningDialog();

            if (!canWrite) 
                return;

            CreateDirectoriesForFile(_locationField.value);
            var graph = ScriptableObject.CreateInstance<DialogueGraph>();
            graph.Name = _nameField.value;
            AssetDatabase.CreateAsset(graph, _locationField.value);
            AssetDatabase.SaveAssets();
            EditorGUIUtility.PingObject(graph);
            _onCreated?.Invoke(graph);
            Created?.Invoke(graph);
            OnCloseClicked();
        }

        private static bool WarningDialog() =>
            EditorUtility.DisplayDialog("Warning", "Creating an asset with this name will result in overwriting an existing one. Are you sure?", "Yes, overwrite it", "No");

        private void UpdateLocation(string filename)
        {
            var pathToAsset = $"Assets/Resources/Dialogues/{filename}/{filename}.asset";

            _warningLabel.Display(ThisAssetAlreadyExisting(pathToAsset));
            _locationField.value = pathToAsset;
        }

        private static void CreateDirectoriesForFile(string path)
        {
            var relativePath = Path.GetRelativePath("Assets", path);
            var targetDirectoryPath = Path.GetDirectoryName(relativePath);
            var fullPathToDirectory = Path.Combine(Application.dataPath, targetDirectoryPath);
            
            if (!Directory.Exists(fullPathToDirectory))
                Directory.CreateDirectory(fullPathToDirectory);
        }
        
        private static bool ThisAssetAlreadyExisting(string path) =>
            AssetDatabase.AssetPathExists(path);
    }
}