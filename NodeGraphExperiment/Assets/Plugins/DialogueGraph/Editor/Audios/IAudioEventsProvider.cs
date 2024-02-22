using System;
using System.Collections.Generic;

namespace Editor.Audios
{
    public interface IAudioEventsProvider
    {
        IEnumerable<string> AllEvents();
        Guid GuidFromName(string eventName);
    }
}