using System;
using System.Collections.Generic;

namespace Runtime.Nodes
{
    [Serializable]
    public class AudioEventNode : BaseDialogueNode
    {
        public List<AudioEventData> Events;
    }

    [Serializable]
    public class AudioEventData
    {
        public string EventName;
        public float Delay;
    }
}