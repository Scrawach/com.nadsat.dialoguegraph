using System;
using System.Collections.Generic;
using System.Linq;
using Nadsat.DialogueGraph.Runtime.Nodes;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Drawing.Controls
{
    public class PlacementViewControl : BaseControl
    {
        private const string Uxml = "UXML/Controls/PlacementDataControl";

        private readonly DropdownField _personDropdown;
        private readonly EnumField _effectEnumField;
        private readonly IntegerField _integerField;
        private readonly Button _removeButton;

        public PlacementViewControl(PlacementData data) : base(Uxml)
        {
            _personDropdown = this.Q<DropdownField>("person-dropdown");
            _effectEnumField = this.Q<EnumField>("effect-field");
            _integerField = this.Q<IntegerField>("priority-field");
            _removeButton = this.Q<Button>("remove-button");

            _personDropdown.RegisterValueChangedCallback(OnPersonDropdownChanged);
            _effectEnumField.RegisterValueChangedCallback(OnEffectFieldChanged);
            _integerField.RegisterValueChangedCallback(OnPositionChanged);

            _personDropdown.value = data.PersonId;
            _effectEnumField.value = data.Effect;
            _integerField.value = data.PlacePosition;
        }

        public event Action<PlacementData> Updated;

        public event Action Closed
        {
            add => _removeButton.clicked += value;
            remove => _removeButton.clicked -= value;
        }

        private void OnPersonDropdownChanged(ChangeEvent<string> evt) => 
            Updated?.Invoke(GetPlacementData());

        private void OnEffectFieldChanged(ChangeEvent<Enum> evt) => 
            Updated?.Invoke(GetPlacementData());

        private void OnPositionChanged(ChangeEvent<int> evt) => 
            Updated?.Invoke(GetPlacementData());

        public PlacementData GetPlacementData() =>
            new PlacementData()
            {
                PersonId = _personDropdown.value,
                Effect = (PlacementEffect)_effectEnumField.value,
                PlacePosition = _integerField.value
            };

        public void InitializePersonDropdown(IEnumerable<string> persons) => 
            _personDropdown.choices = persons.ToList();
    }
}