using System;
using System.Collections.Generic;

namespace Editor.Audios
{
    public class DebugEventsProvider : IAudioEventsProvider
    {
        public IEnumerable<string> AllEvents()
        {
            yield return "Test Event 1";
            yield return "Test Event 2";
        }

        public Guid GuidFromName(string name) =>
            Guid.Empty;
    }
}