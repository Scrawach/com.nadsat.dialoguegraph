using System;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Drawing.Controls
{
    public class PhraseTextControl : BaseControl
    {
        private const string Uxml = "UXML/Controls/PhraseTextControl";
        private readonly Button _close;
        private readonly Label _description;

        private readonly Label _title;

        public PhraseTextControl(string title, string description) : this()
        {
            _title.text = title;
            _description.text = description;
        }

        public PhraseTextControl() : base(Uxml)
        {
            _title = this.Q<Label>("title");
            _description = this.Q<Label>("description");
            _close = this.Q<Button>("close-button");

            _title.selection.isSelectable = true;
            _description.selection.isSelectable = true;
        }

        public event Action Closed
        {
            add => _close.clicked += value;
            remove => _close.clicked -= value;
        }

        public new class UxmlFactory : UxmlFactory<PhraseTextControl, UxmlTraits> { }
    }
}