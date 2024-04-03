using System;
using System.Text;
using Nadsat.DialogueGraph.Runtime.Nodes;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Drawing.Nodes
{
    public class PlacementNodeView : BaseNodeView<PlacementNode>
    {
        private const string UxmlPath = "UXML/PlacementNodeView";

        private readonly Label _phraseTextLabel;
        
        public PlacementNodeView() : base(UxmlPath) => 
            _phraseTextLabel = this.Q<Label>("description-label");

        protected override void OnModelChanged() => 
            _phraseTextLabel.text = ConvertPlacementNodeToString(Model);

        private string ConvertPlacementNodeToString(PlacementNode node)
        {
            var builder = new StringBuilder();
            
            foreach (var placementData in node.Data)
            {
                builder.Append(LineFromPlacementData(placementData));
                builder.Append(Environment.NewLine);
            }

            return builder.ToString();
        }
        
        private string LineFromPlacementData(PlacementData data) => 
            $"{data.PersonId} [{data.PlacePosition}] - {data.Effect}";
    }
}