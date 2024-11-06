using Nadsat.DialogueGraph.Runtime.Nodes;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Drawing.Nodes
{
    public class BackgroundImageNodeView : BaseNodeView<BackgroundImageNode>
    {
        private const string UxmlPath = "UXML/BackgroundImageNodeView";

        private readonly VisualElement _image;
        private readonly VisualElement _imageContainer;

        public BackgroundImageNodeView() : base(UxmlPath)
        {
            _image = this.Q<VisualElement>("image");
            _imageContainer = this.Q<VisualElement>("image-container");
        }
        
        protected override void OnModelChanged() => 
            Draw(Model);

        private void Draw(BackgroundImageNode model)
        {
            SetImage(model);
        }
        
        private void SetImage(BackgroundImageNode data)
        {
            if (data == null || string.IsNullOrWhiteSpace(data.PathToImage))
            {
                _imageContainer.style.display = DisplayStyle.None;
            }
            else
            {
                var image = AssetDatabase.LoadAssetAtPath<Sprite>(data.PathToImage);
                _imageContainer.style.display = DisplayStyle.Flex;
                _image.style.backgroundImage = new StyleBackground(image);
            }
        }
    }
}