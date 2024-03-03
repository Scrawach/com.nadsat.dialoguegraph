using System;
using System.Collections.Generic;
using System.Linq;

namespace Nadsat.DialogueGraph.Editor.Audios.Wwise
{
#if HAS_WWISE
    public class WwiseAudioEventsProvider : IAudioEventsProvider
    {
        public IEnumerable<string> AllEvents() =>
            AllWwiseEvents().Select(audioEvent => audioEvent.Name);

        public Guid GuidFromName(string eventName) =>
            AllWwiseEvents().First(audioEvent => audioEvent.Name == eventName).Guid;

        private static IEnumerable<AkWwiseProjectData.Event> AllWwiseEvents() =>
            AkWwiseProjectInfo.GetData().EventWwu.SelectMany(eventUnit => eventUnit.List);  
    }
#endif
}