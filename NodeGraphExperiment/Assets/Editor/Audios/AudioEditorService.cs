using UnityEngine;

namespace Editor.Audios
{
    public class AudioEditorService
    {
        private readonly AudioEventsProvider _audioEventsProvider;

        public AudioEditorService(AudioEventsProvider audioEventsProvider) =>
            _audioEventsProvider = audioEventsProvider;

        public void Play(string eventName)
        {
#if UseWwise
            var guid = _audioEventsProvider.GetGuidFromName(eventName);
            AkWaapiUtilities.TogglePlayEvent(WwiseObjectType.Event, guid);
#endif
        }

        public void Stop(string eventName)
        {
            Debug.Log($"stop {eventName}");
        }

        public void Seek(string eventName, float progress)
        {
            Debug.Log($"seek {eventName} to {progress}");
        }

        public void StopAll()
        {
            Debug.Log($"stop all");
        }
    }
}