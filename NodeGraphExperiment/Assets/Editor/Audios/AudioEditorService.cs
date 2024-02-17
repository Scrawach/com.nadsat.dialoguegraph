using UnityEngine;

namespace Editor.Audios
{
    public class AudioEditorService
    {
        public void Play(string eventName)
        {
            Debug.Log($"play {eventName}");
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