using System;
using Nadsat.DialogueGraph.Editor.Extensions;
using Nadsat.DialogueGraph.Runtime.Data;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Drawing.Controls
{
    public class ChoiceCardControl : CardControl
    {
        private const string Uxml = "UXML/Controls/ChoiceCardControl";

        private readonly Toggle _lockedToggle;
        private readonly TextField _unlockCondition;
        
        public ChoiceCardControl(ChoiceData choice, string description)
            : base(choice.ChoiceId, description, Uxml)
        {
            _lockedToggle = this.Q<Toggle>("locked-toggle");
            _unlockCondition = this.Q<TextField>("unlock-condition");

            _lockedToggle.value = choice.IsLocked;
            _unlockCondition.value = choice.UnlockedCondition;

            _lockedToggle.RegisterValueChangedCallback(OnToggleChanged);
            _unlockCondition.RegisterCallback<FocusOutEvent>(OnEditTextFinished, TrickleDown.TrickleDown);


            _unlockCondition.Display(_lockedToggle.value);
        }
        
        public event Action<bool> IsLockedToggled;

        public event Action<string> UnlockConditionChanged; 

        private void OnToggleChanged(ChangeEvent<bool> evt)
        {
            IsLockedToggled?.Invoke(evt.newValue);
            _unlockCondition.Display(evt.newValue);
        }
        
        private void OnEditTextFinished(FocusOutEvent evt) => 
            UnlockConditionChanged?.Invoke(_unlockCondition.value);
    }
}