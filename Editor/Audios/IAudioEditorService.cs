using System;

namespace Nadsat.DialogueGraph.Editor.Audios
{
    public interface IAudioEditorService
    {
        event Action<float> PlayingProgressChanged; 
        void Initialize();
        void Update();
        void PlayEvent(string eventName);
        void StopEvent(string eventName);
        void SeekOnEvent(string eventName, float progress);
        void StopAll();
    }
}