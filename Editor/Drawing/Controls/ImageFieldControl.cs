using System;
using Nadsat.DialogueGraph.Editor.Extensions;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Nadsat.DialogueGraph.Editor.Drawing.Controls
{
    public class ImageFieldControl : BaseControl
    {
        private const string Uxml = "UXML/Controls/ImageFieldControl";
        private readonly Button _close;
        private readonly VisualElement _image;

        private readonly ObjectField _imageField;

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
            _image.Display(true);
            _image.style.backgroundImage = new StyleBackground(sprite);
        }

        public void RemoveImage()
        {
            _imageField.value = null;
            _image.Display(false);
        }

        private void OnFieldChanged(ChangeEvent<Object> evt)
        {
            var sprite = evt.newValue as Sprite;
            SetImage(sprite);
            Selected?.Invoke(sprite);
        }

        public new class UxmlFactory : UxmlFactory<ImageFieldControl, UxmlTraits> { }
    }
}