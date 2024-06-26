using Nadsat.DialogueGraph.Editor.AssetManagement;
using Nadsat.DialogueGraph.Editor.Extensions;
using Nadsat.DialogueGraph.Runtime.Nodes;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Drawing.Nodes
{
    public class DialogueNodeView : BaseNodeView<DialogueNode>
    {
        private const string UxmlPath = "UXML/DialogueNodeView";
        private readonly VisualElement _avatar;
        private readonly DialogueDatabase _database;

        private readonly VisualElement _header;
        private readonly VisualElement _image;
        private readonly VisualElement _imageContainer;

        private readonly Label _personNameLabel;
        private readonly PhraseRepository _phrases;
        private readonly Label _phraseTextLabel;
        private readonly Label _phraseTitleLabel;

        public DialogueNodeView(PhraseRepository phrases, DialogueDatabase database)
            : base(UxmlPath)
        {
            _phrases = phrases;
            _database = database;

            _personNameLabel = this.Q<Label>("person-name-label");
            _phraseTitleLabel = this.Q<Label>("title-label");
            _phraseTextLabel = this.Q<Label>("description-label");
            _header = this.Q<VisualElement>("header");
            _avatar = this.Q<VisualElement>("avatar");
            _image = this.Q<VisualElement>("image");
            _imageContainer = this.Q<VisualElement>("image-container");
        }

        protected override void OnModelChanged() =>
            Draw(Model);

        private void Draw(DialogueNode model)
        {
            SetPerson(model.PersonId);
            SetPhrase(model.PhraseId);
            SetImage(model.BackgroundImage);
        }

        private void SetPhrase(string phraseId)
        {
            if (string.IsNullOrWhiteSpace(phraseId))
            {
                _phraseTitleLabel.text = "none";
                _phraseTextLabel.text = string.Empty;
            }
            else
            {
                var phrase = _phrases.Get(phraseId);
                _phraseTitleLabel.text = phraseId;
                _phraseTextLabel.text = phrase;
            }
        }

        private void SetPerson(string personId)
        {
            var person = _database.Get(personId);
            var avatar = AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GetAssetPath(person.Icon));
            _personNameLabel.text = person.Name;
            _header.style.backgroundColor = person.Color;

            if (avatar != null)
            {
                _avatar.Display(true);
                _avatar.style.backgroundImage = new StyleBackground(avatar);
            }
            else
            {
                _avatar.Display(false);
            }
        }

        private void SetImage(BackgroundImageData data)
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