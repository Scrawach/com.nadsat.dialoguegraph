using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Editor
{
    public class ImageItemView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<ImageItemView, UxmlTraits> { }

        private readonly ObjectField _imageField;
        private readonly VisualElement _image;
        private readonly Button _closeButton;
        
        public ImageItemView()
        {
            var uxml = Resources.Load<VisualTreeAsset>("UXML/ImageItem");
            uxml.CloneTree(this);

            _imageField = this.Q<ObjectField>("image-field");
            _imageField.RegisterValueChangedCallback(OnImageFieldChanged);
            _image = this.Q<VisualElement>("image");

            _closeButton = this.Q<Button>("close-button");
        }

        public event Action<Sprite> ImageUploaded;
        
        public event Action Closed
        {
            add => _closeButton.clicked += value;
            remove => _closeButton.clicked -= value;
        }

        public void SetImage(Sprite sprite)
        {
            _imageField.value = sprite;
            _image.style.display = DisplayStyle.Flex;
            _image.style.backgroundImage = new StyleBackground(sprite);
        }

        private void OnImageFieldChanged(ChangeEvent<Object> evt)
        {
            var sprite = evt.newValue as Sprite;
            
            if (sprite)
            {
                SetImage(sprite);
                ImageUploaded?.Invoke(sprite);
            }
        }
    }
}