using System;
using System.Collections.Generic;
using System.Linq;

namespace Editor.Audios
{
    public class AudioEventsProvider
    {
        public IEnumerable<string> GetEvents() =>
            AllEvents().Select(audioEvent => audioEvent.Name);

        public Guid GetGuidFromName(string name) =>
            AllEvents().First(audioEvent => audioEvent.Name == name).Guid;

        private IEnumerable<AkWwiseProjectData.Event> AllEvents() =>
            AkWwiseProjectInfo.GetData().EventWwu.SelectMany(eventUnit => eventUnit.List);
    }
}