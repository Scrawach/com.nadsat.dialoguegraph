namespace Nadsat.DialogueGraph.Editor.Audios
{
    public interface IAudioEditorService
    {
        void Initialize();
        void PlayEvent(string eventName);
        void StopEvent(string eventName);
        void SeekOnEvent(string eventName, float progress);
        void StopAll();
    }
}