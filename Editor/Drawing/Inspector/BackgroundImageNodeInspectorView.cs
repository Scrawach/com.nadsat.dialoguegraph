using Nadsat.DialogueGraph.Editor.Drawing.Controls;
using Nadsat.DialogueGraph.Runtime.Nodes;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Drawing.Inspector
{
    public class BackgroundImageNodeInspectorView : BaseControl
    {
        private const string Uxml = "UXML/BackgroundImageNodeInspectorView";

        private readonly VisualElement _imagesContainer;
        private readonly Button _addImageButton;

        private readonly BackgroundImageNode _node;
        
        private ImageFieldControl _activeImage;
        
        public BackgroundImageNodeInspectorView(BackgroundImageNode node) : base(Uxml)
        {
            _node = node;
            
            _imagesContainer = this.Q<VisualElement>("images-container");
            _addImageButton = this.Q<Button>("add-image-button");
            _addImageButton.clicked += OnAddImageButtonClicked;
            
            _node.Changed += OnNodeUpdated;
            OnNodeUpdated();
        }

        private void OnNodeUpdated()
        {
            if (HasImage(_node))
                SetImage(_node);
        }
        
        private void OnAddImageButtonClicked()
        {
            _addImageButton.style.display = DisplayStyle.None;
            SetImage(new BackgroundImageNode());
        }
        
        private bool HasImage(BackgroundImageNode data) => 
            data != null && !string.IsNullOrEmpty(data.PathToImage);
        
        private void SetImage(BackgroundImageNode data)
        {
            if (_activeImage != null)
                _imagesContainer.Remove(_activeImage);

            var item = new ImageFieldControl();
            _activeImage = item;

            if (!string.IsNullOrWhiteSpace(data.PathToImage))
            {
                _addImageButton.style.display = DisplayStyle.None;
                item.SetImage(AssetDatabase.LoadAssetAtPath<Sprite>(data.PathToImage));
            }

            _imagesContainer.Add(item);

            item.Closed += () =>
            {
                _activeImage = null;
                _imagesContainer.Remove(item);
                _addImageButton.style.display = DisplayStyle.Flex;
                _node.SetBackgroundImage(data.PathToImage);
            };

            item.Selected += sprite =>
            {
                var pathToSprite = AssetDatabase.GetAssetPath(sprite);

                if (Validate(pathToSprite))
                {
                    _node.PathToImage = pathToSprite;
                    _node.NotifyChanged();
                }
                else
                {
                    item.RemoveImage();
                    _node.SetBackgroundImage(null);
                }
            };
        }
        
        private static bool Validate(string pathToSprite)
        {
            var inResourcesFolder = pathToSprite.Contains("Resources");

            if (!inResourcesFolder)
                EditorUtility.DisplayDialog("Warning", "Sprite should be in Resources/ folder!", "Ok");
            
            return inResourcesFolder;
        }
    }
}