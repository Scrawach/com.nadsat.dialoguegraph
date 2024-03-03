using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Nadsat.DialogueGraph.Editor.Audios.Wwise
{
#if HAS_WWISE
    public class WwiseAudioEditorService : IAudioEditorService
    {
        private const string PathToInitializer = "WwiseGlobal";
        private readonly IAudioEventsProvider _audioEventsProvider;

        private AkInitializer _initializer;
        private uint _playingId;

        public WwiseAudioEditorService(IAudioEventsProvider audioEventsProvider) =>
            _audioEventsProvider = audioEventsProvider;

        public void Initialize()
        {
            _initializer = Resources.Load<AkInitializer>(PathToInitializer);
            AkSoundEngineController.Instance.Init(_initializer);
            AkSoundEngine.RegisterGameObj(_initializer.gameObject);
            AkBankManager.LoadBankAsync("Init", OnBankLoaded);
            AkBankManager.LoadBankAsync("L1_Intro", OnBankLoaded);
            AkBankManager.LoadBankAsync("L2_Tutor", OnBankLoaded);
        }

        private void OnBankLoaded(uint in_bankid, IntPtr in_inmemorybankptr, AKRESULT in_eloadresult, object in_cookie)
        {
            Debug.Log($"Loaded bank {in_bankid} with {in_eloadresult}");
        }

        public void PlayEvent(string eventName)
        {
            var flags = (uint)AkCallbackType.AK_EnableGetSourcePlayPosition;
            _playingId = AkSoundEngine.PostEvent(eventName, _initializer.gameObject, flags, null, null);
        }

        public void StopEvent(string eventName)
        {
            AkSoundEngine.StopAll();
            AkSoundEngine.StopPlayingID(_playingId);
        }

        public void SeekOnEvent(string eventName, float progress)
        {
            AkSoundEngine.StopPlayingID(_playingId);
            PlayEvent(eventName);
            AkSoundEngine.SeekOnEvent(eventName, _initializer.gameObject, progress);
        }

        public void StopAll() => 
            AkSoundEngine.StopAll();
        
    }
#endif
}