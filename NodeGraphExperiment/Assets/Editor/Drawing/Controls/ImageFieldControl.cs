using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Editor.Drawing.Controls
{
    public class ImageFieldControl : BaseControl
    {
        public new class UxmlFactory : UxmlFactory<ImageFieldControl, UxmlTraits> { }

        private const string Uxml = "UXML/Controls/ImageFieldControl";

        private readonly ObjectField _imageField;
        private readonly VisualElement _image;
        private readonly Button _close;

        public ImageFieldControl() : base(Uxml)
        {
            _imageField = this.Q<ObjectField>("image-field");
            _image = this.Q<VisualElement>("image");
            _close = this.Q<Button>("close-button");

            _imageField.RegisterValueChangedCallback(OnFieldChanged);
        }

        public event Action Closed
        {
            add => _close.clicked += value;
            remove => _close.clicked -= value;
        }

        public event Action<Sprite> Selected;

        public void SetImage(Sprite sprite)
        {
            _imageField.value = sprite;
            _image.style.display = DisplayStyle.Flex;
            _image.style.backgroundImage = new StyleBackground(sprite);
        }

        private void OnFieldChanged(ChangeEvent<Object> evt)
        {
            var sprite = evt.newValue as Sprite;
            SetImage(sprite);
            Selected?.Invoke(sprite);
        }
    }
}