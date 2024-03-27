using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Drawing.Controls
{
    public class SwitchFieldControl : CardControl
    {
        private const string Uxml = "UXML/Controls/SwitchFieldControl";

        private readonly Button _findButton;

        public SwitchFieldControl(string branchCondition) : base(string.Empty, branchCondition, Uxml) => 
            _findButton = this.Q<Button>("find-button");

        public event Action FindClicked
        {
            add => _findButton.clicked += value;
            remove => _findButton.clicked -= value;
        }
    }
}