using System;
using System.Collections.Generic;

namespace Runtime.Nodes
{
    [Serializable]
    public class AudioEventNode : BaseDialogueNode
    {
        public List<AudioEventData> Events = new();

        public void AddEvent(AudioEventData data)
        {
            Events.Add(data);
            NotifyChanged();
        }

        public void RemoveEvent(AudioEventData data)
        {
            Events.Remove(data);
            NotifyChanged();
        }
    }

    [Serializable]
    public class AudioEventData
    {
        public string EventName;
        public float Delay;
    }
}