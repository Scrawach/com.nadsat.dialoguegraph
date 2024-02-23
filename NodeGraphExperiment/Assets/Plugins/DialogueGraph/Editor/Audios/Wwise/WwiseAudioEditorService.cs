namespace Nadsat.DialogueGraph.Editor.Audios.Wwise
{
#if HAS_WWISE
    public class WwiseAudioEditorService : IAudioEditorService
    {
        private readonly IAudioEventsProvider _audioEventsProvider;

        public WwiseAudioEditorService(IAudioEventsProvider audioEventsProvider) =>
            _audioEventsProvider = audioEventsProvider;

        public void PlayEvent(string eventName)
        {
            var guid = _audioEventsProvider.GuidFromName(eventName);
            AkWaapiUtilities.TogglePlayEvent(WwiseObjectType.Event, guid);
        }

        public void StopEvent(string eventName)
        {
        }

        public void SeekOnEvent(string eventName, float progress)
        {
        }

        public void StopAll()
        {
        }
    }
#endif
}