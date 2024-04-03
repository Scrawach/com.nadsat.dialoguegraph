using System;
using System.Collections.Generic;

namespace Nadsat.DialogueGraph.Runtime.Nodes
{
    [Serializable]
    public class PlacementNode : BaseDialogueNode
    {
        public List<PlacementData> Data = new();

        public void AddPlacement(PlacementData data)
        {
            Data.Add(data);
            NotifyChanged();
        }

        public bool RemovePlacement(PlacementData data)
        {
            var result = Data.Remove(data);
            NotifyChanged();
            return result;
        }
    }

    [Serializable]
    public class PlacementData
    {
        public string PersonId;
        public int PlacePosition;
        public PlacementEffect Effect;
    }

    public enum PlacementEffect
    {
        Entering = 0,
        Leaving = 1,
    }
}