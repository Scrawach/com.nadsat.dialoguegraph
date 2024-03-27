using System;
using Nadsat.DialogueGraph.Editor.Extensions;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Drawing.Controls
{
    public class CardControl : BaseControl
    {
        private const string Uxml = "UXML/Controls/CardControl";

        private readonly Button _close;
        private readonly Label _title;
        private readonly Label _description;
        private readonly TextField _textField;

        public CardControl(string title, string description, string uxml = Uxml) 
            : this(uxml)
        {
            _title.text = title;
            _description.text = description;
        }
        
        public CardControl() : this(Uxml) { }

        public CardControl(string uxml) : base(uxml)
        {
            _title = this.Q<Label>("title");
            _description = this.Q<Label>("description");
            _textField = this.Q<TextField>("text-field");
            _close = this.Q<Button>("close-button");

            var textInput = _textField.Q(TextInputBaseField<string>.textInputUssName);
            textInput.RegisterCallback<FocusOutEvent>(OnEditTextFinished, TrickleDown.TrickleDown);
            textInput.RegisterCallback<KeyDownEvent>(OnKeyDownEvent, TrickleDown.TrickleDown);
            _description.RegisterCallback<MouseDownEvent>(OnMouseDownEvent);

            _title.selection.isSelectable = true;
            _description.selection.isSelectable = true;
        }

        public event Action<string> TextEdited;

        public event Action Closed
        {
            add => _close.clicked += value;
            remove => _close.clicked -= value;
        }

        public void StartEdit() =>
            OpenTextEditor();

        private void OnKeyDownEvent(KeyDownEvent evt)
        {
            if (evt.keyCode is KeyCode.Return or KeyCode.KeypadEnter)
            {
                OnEditTextFinished(null);
                evt.StopPropagation();
            }
        }

        private void OnEditTextFinished(FocusOutEvent evt)
        {
            _description.Display(true);
            _textField.Display(false);

            var text = _textField.text;
            text = RemoveNewlineSymbol(text);
            _description.text = text;
            TextEdited?.Invoke(text);
        }

        private string RemoveNewlineSymbol(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            var lastIndex = _textField.text.Length - 1;
            if (text[lastIndex] == '\n')
                text = text.Remove(lastIndex, 1);

            return text;
        }

        private void OnMouseDownEvent(MouseDownEvent e)
        {
            if (e.clickCount != 2 || e.button != 0)
                return;
            OpenTextEditor();
            e.PreventDefault();
        }

        private void OpenTextEditor()
        {
            _textField.SetValueWithoutNotify(_description.text);
            _textField.Display(true);
            _description.Display(false);
            _textField.Q(TextInputBaseField<string>.textInputUssName).Focus();
            _textField.textSelection.SelectAll();
        }

        public void Add(string text) => 
            _description.text += " " + text;

        public new class UxmlFactory : UxmlFactory<CardControl, UxmlTraits> { }
    }
}