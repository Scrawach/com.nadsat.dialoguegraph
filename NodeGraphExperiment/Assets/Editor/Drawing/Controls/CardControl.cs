using System;
using Editor.Extensions;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Drawing.Controls
{
    public class CardControl : BaseControl
    {
        private const string Uxml = "UXML/Controls/CardControl";

        private readonly Button _close;
        public readonly Label Description;
        public readonly TextField TextField;

        public readonly Label Title;

        public CardControl(string title, string description) : this()
        {
            Title.text = title;
            Description.text = description;
        }

        public CardControl() : base(Uxml)
        {
            Title = this.Q<Label>("title");
            Description = this.Q<Label>("description");
            TextField = this.Q<TextField>("text-field");
            _close = this.Q<Button>("close-button");

            var textInput = TextField.Q(TextInputBaseField<string>.textInputUssName);
            textInput.RegisterCallback<FocusOutEvent>(OnEditTextFinished, TrickleDown.TrickleDown);
            textInput.RegisterCallback<KeyDownEvent>(OnKeyDownEvent, TrickleDown.TrickleDown);
            Description.RegisterCallback<MouseDownEvent>(OnMouseDownEvent);

            Title.selection.isSelectable = true;
            Description.selection.isSelectable = true;
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
            if (evt.keyCode == KeyCode.Return)
            {
                OnEditTextFinished(null);
                evt.StopPropagation();
            }
        }

        private void OnEditTextFinished(FocusOutEvent evt)
        {
            Description.Display(true);
            TextField.Display(false);

            var text = TextField.text;
            text = RemoveNewlineSymbol(text);
            Description.text = text;
            TextEdited?.Invoke(text);
        }

        private string RemoveNewlineSymbol(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            var lastIndex = TextField.text.Length - 1;
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
            TextField.SetValueWithoutNotify(Description.text);
            TextField.Display(true);
            Description.Display(false);
            TextField.Q(TextInputBaseField<string>.textInputUssName).Focus();
            TextField.textSelection.SelectAll();
        }

        public new class UxmlFactory : UxmlFactory<CardControl, UxmlTraits> { }
    }
}