using System;
using System.Collections.Generic;
using System.Linq;

namespace Editor.Audios
{
    public class AudioEventsProvider
    {

#if HasWwise
        public IEnumerable<string> GetEvents() =>
            AllEvents().Select(audioEvent => audioEvent.Name);

        public Guid GetGuidFromName(string name) =>
            AllEvents().First(audioEvent => audioEvent.Name == name).Guid;

        private static IEnumerable<AkWwiseProjectData.Event> AllEvents() =>
            AkWwiseProjectInfo.GetData().EventWwu.SelectMany(eventUnit => eventUnit.List);     
#else
        public IEnumerable<string> GetEvents()
        {
            yield return "Test Event 1";
            yield return "Test Event 2";
        }

        public Guid GetGuidFromName(string name) =>
            Guid.Empty;
#endif
    }
}