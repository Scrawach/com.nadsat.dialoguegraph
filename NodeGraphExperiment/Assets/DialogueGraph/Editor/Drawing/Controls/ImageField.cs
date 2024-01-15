using System;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace DialogueGraph.Editor.Drawing.Controls
{
    public class ImageField : BaseControl
    {
        public new class UxmlFactory : UxmlFactory<ImageField, UxmlTraits> { }

        private const string Uxml = "UXML/ImageField";

        private readonly ObjectField _field;
        private readonly VisualElement _image;
        private readonly Button _close;

        public ImageField() : base(Uxml)
        {
            _field = this.Q<ObjectField>("image-field");
            _image = this.Q<VisualElement>("image");
            _close = this.Q<Button>("close-button");

            _field.RegisterValueChangedCallback(OnFieldChanged);
        }

        public event Action Closed
        {
            add => _close.clicked += value;
            remove => _close.clicked -= value;
        }

        public event Action<Sprite> Selected;

        public void SetImage(Sprite sprite)
        {
            _field.value = sprite;
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