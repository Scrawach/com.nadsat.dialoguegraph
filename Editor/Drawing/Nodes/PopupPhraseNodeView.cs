using Nadsat.DialogueGraph.Editor.AssetManagement;
using Nadsat.DialogueGraph.Runtime.Nodes;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Drawing.Nodes
{
    public class PopupPhraseNodeView : BaseNodeView<PopupPhraseNode>
    {
        private const string UxmlPath = "UXML/PopupPhraseNodeView";

        private readonly PhraseRepository _phrases;

        private readonly Label _phraseTitleLabel;
        private readonly Label _phraseTextLabel;

        public PopupPhraseNodeView(PhraseRepository phrases) 
            : base(UxmlPath)
        {
            _phrases = phrases;
            
            _phraseTitleLabel = this.Q<Label>("title-label");
            _phraseTextLabel = this.Q<Label>("description-label");
        }

        protected override void OnModelChanged()
        {
            if (string.IsNullOrWhiteSpace(Model.PhraseId))
            {
                _phraseTitleLabel.text = "none";
                _phraseTextLabel.text = string.Empty;
            }
            else
            {
                _phraseTitleLabel.text = Model.PhraseId;
                _phraseTextLabel.text = _phrases.Get(Model.PhraseId);
            }
        }
    }
}