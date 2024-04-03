using System.Collections.Generic;
using System.Linq;
using Nadsat.DialogueGraph.Editor.AssetManagement;
using Nadsat.DialogueGraph.Editor.Drawing.Controls;
using Nadsat.DialogueGraph.Runtime.Nodes;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Drawing.Inspector
{
    public class PlacementNodeInspectorView : BaseControl
    {
        private const string Uxml = "UXML/PlacementNodeInspectorView";

        private readonly Label _guidLabel;
        private readonly Button _addDataButton;
        private readonly VisualElement _container;

        private PlacementNode _node;
        private readonly DialogueDatabase _database;

        public PlacementNodeInspectorView(PlacementNode node, DialogueDatabase database) : base(Uxml)
        {
            _node = node;
            _database = database;
            _guidLabel = this.Q<Label>("guid-label");
            _addDataButton = this.Q<Button>("add-button");
            _container = this.Q<VisualElement>("placement-container");

            _addDataButton.clicked += OnAddClicked;
            
            _node.Changed += OnNodeUpdated;
            OnNodeUpdated();
        }

        private void OnNodeUpdated()
        {
            _guidLabel.text = _node.Guid;
            _container.Clear();
            foreach (var data in _node.Data)
            {
                CreatePlacementData(data);
            }
        }

        private PlacementViewControl CreatePlacementData(PlacementData data)
        {
            var control = new PlacementViewControl(data);
            control.InitializePersonDropdown(_database.All());
            _container.Add(control);
            
            control.Closed += () =>
            {
                _node.RemovePlacement(data);
            };

            control.Updated += (updatedData) =>
            {
                data.Effect = updatedData.Effect;
                data.PersonId = updatedData.PersonId;
                data.PlacePosition = updatedData.PlacePosition;
                _node.NotifyChanged();
            };
            
            return control;
        }
        
        private void OnAddClicked() => 
            _node.AddPlacement(new PlacementData());
    }
}