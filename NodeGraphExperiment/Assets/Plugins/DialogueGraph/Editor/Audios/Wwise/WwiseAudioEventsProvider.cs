namespace Nadsat.DialogueGraph.Editor.Audios.Wwise
{
#if HAS_WWISE
    public class WwiseAudioEventsProvider : IAudioEventsProvider
    {
        public IEnumerable<string> AllEvents() =>
            AllEvents().Select(audioEvent => audioEvent.Name);

        public Guid GuidFromName(string eventName) =>
            AllEvents().First(audioEvent => audioEvent.Name == name).Guid;

        private static IEnumerable<AkWwiseProjectData.Event> AllEvents() =>
            AkWwiseProjectInfo.GetData().EventWwu.SelectMany(eventUnit => eventUnit.List);  
    }
#endif
}