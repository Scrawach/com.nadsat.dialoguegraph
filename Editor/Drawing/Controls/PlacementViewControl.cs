using System;
using System.Collections.Generic;
using System.Linq;
using Nadsat.DialogueGraph.Editor.Data;
using Nadsat.DialogueGraph.Editor.Extensions;
using Nadsat.DialogueGraph.Runtime.Nodes;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Drawing.Controls
{
    public class PlacementViewControl : BaseControl
    {
        private const string Uxml = "UXML/Controls/PlacementDataControl";

        private readonly DropdownField _personDropdown;
        private readonly EnumField _effectEnumField;
        private readonly EnumField _placeField;
        private readonly Button _removeButton;

        public PlacementViewControl(PlacementData data) : base(Uxml)
        {
            _personDropdown = this.Q<DropdownField>("person-dropdown");
            _effectEnumField = this.Q<EnumField>("effect-field");
            _placeField = this.Q<EnumField>("place-field");
            _removeButton = this.Q<Button>("remove-button");

            _personDropdown.RegisterValueChangedCallback(OnPersonDropdownChanged);
            _effectEnumField.RegisterValueChangedCallback(OnEffectFieldChanged);
            _placeField.RegisterValueChangedCallback(OnPlaceFieldChanged);

            _personDropdown.value = data.PersonId;
            _effectEnumField.value = data.Effect;
            _placeField.value = (PhoneCallPlacement) data.PlacePosition;
            ShowOrHidePlaceSetting((PlacementEffect) _effectEnumField.value);
        }
        
        public event Action<PlacementData> Updated;

        public event Action Closed
        {
            add => _removeButton.clicked += value;
            remove => _removeButton.clicked -= value;
        }

        private void OnPersonDropdownChanged(ChangeEvent<string> evt) => 
            Updated?.Invoke(GetPlacementData());

        private void OnEffectFieldChanged(ChangeEvent<Enum> evt)
        {
            ShowOrHidePlaceSetting((PlacementEffect) evt.newValue);
            Updated?.Invoke(GetPlacementData());
        }

        private void OnPlaceFieldChanged(ChangeEvent<Enum> evt) => 
            Updated?.Invoke(GetPlacementData());

        public PlacementData GetPlacementData() =>
            new PlacementData()
            {
                PersonId = _personDropdown.value,
                Effect = (PlacementEffect)_effectEnumField.value,
                PlacePosition = Convert.ToInt32(_placeField.value)
            };

        private void ShowOrHidePlaceSetting(PlacementEffect newPlace)
        {
            if (newPlace != PlacementEffect.Entering)
            {
                _placeField.value = (PhoneCallPlacement)0;
                _placeField.Display(false);
            }
            else
            {
                _placeField.Display(true);
            }
        }

        
        public void InitializePersonDropdown(IEnumerable<string> persons) => 
            _personDropdown.choices = persons.ToList();
    }
}