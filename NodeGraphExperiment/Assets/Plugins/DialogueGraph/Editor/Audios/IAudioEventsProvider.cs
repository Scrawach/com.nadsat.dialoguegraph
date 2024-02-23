using System;
using System.Collections.Generic;

namespace Nadsat.DialogueGraph.Editor.Audios
{
    public interface IAudioEventsProvider
    {
        IEnumerable<string> AllEvents();
        Guid GuidFromName(string eventName);
    }
}