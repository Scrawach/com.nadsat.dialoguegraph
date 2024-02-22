using UnityEngine;

namespace Editor.Audios
{
    public class DebugAudioEditorService : IAudioEditorService
    {
        public void PlayEvent(string eventName) =>
            Debug.Log($"Play event {eventName}");

        public void StopEvent(string eventName) =>
            Debug.Log($"Stop event {eventName}");

        public void SeekOnEvent(string eventName, float progress) =>
            Debug.Log($"Seek event {eventName} on {progress}");

        public void StopAll() =>
            Debug.Log($"Stop All events");
    }
}