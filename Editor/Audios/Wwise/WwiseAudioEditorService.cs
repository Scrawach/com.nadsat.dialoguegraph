using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nadsat.DialogueGraph.Editor.Audios.Wwise
{
#if HAS_WWISE
    public class WwiseAudioEditorService : IAudioEditorService
    {
        private const string PathToInitializer = "WwiseGlobal";
        private readonly IAudioEventsProvider _audioEventsProvider;

        private AkInitializer _initializer;
        private uint _playingId;
        private AkWwiseProjectData.Event _playingEvent;

        public WwiseAudioEditorService(IAudioEventsProvider audioEventsProvider) =>
            _audioEventsProvider = audioEventsProvider;

        public event Action<float> PlayingProgressChanged;

        public void Initialize()
        {
            _initializer = Resources.Load<AkInitializer>(PathToInitializer);
            AkSoundEngineController.Instance.Init(_initializer);
            AkSoundEngine.RegisterGameObj(_initializer.gameObject);
            AkSoundEngine.SetCurrentLanguage("English");
            LoadAllBanks();
        }

        private static void LoadAllBanks()
        {
            AkBankManager.LoadInitBank();
            foreach (var bankName in AllBankNames()) 
                AkBankManager.LoadBankAsync(bankName);
        }
        
        private static IEnumerable<string> AllBankNames() => 
            from bankUnit in AkWwiseProjectInfo.GetData().BankWwu 
            from information in bankUnit.List 
            select information.Name;

        public void Update()
        {
            if (_playingId == 0)
                return;

            var progress = GetCurrentProgress();
            PlayingProgressChanged?.Invoke(progress);
        }
        
        public void PlayEvent(string eventName)
        {
            //const uint flags = (uint) AkCallbackType.AK_EnableGetSourcePlayPosition;
            const uint flags =   (uint)AkCallbackType.AK_MusicSyncAll 
                               | (uint)AkCallbackType.AK_EnableGetMusicPlayPosition 
                               | (uint)AkCallbackType.AK_EnableGetSourcePlayPosition
                               | (uint)AkCallbackType.AK_EnableGetSourceStreamBuffering 
                               | (uint)AkCallbackType.AK_CallbackBits;
            _playingId = AkSoundEngine.PostEvent(eventName, _initializer.gameObject, flags, OnCallback, null);
            var eventId = AkSoundEngine.GetEventIDFromPlayingID(_playingId);
            _playingEvent =  AkWwiseProjectInfo.GetData().GetEventInfo(eventId);
        }

        private void OnCallback(object in_cookie, AkCallbackType in_type, AkCallbackInfo in_info)
        {
            // Need callback, because without OnCallback can't get source play position, idk why (?!)
        }
        
        public void StopEvent(string eventName)
        {
            AkSoundEngine.StopAll();
            AkSoundEngine.StopPlayingID(_playingId);
            _playingId = 0;
        }

        public void SeekOnEvent(string eventName, float progress)
        {
            AkSoundEngine.StopPlayingID(_playingId);
            PlayEvent(eventName);
            AkSoundEngine.SeekOnEvent(eventName, _initializer.gameObject, progress);
        }

        public void StopAll()
        {
            AkSoundEngine.StopAll();
            _playingId = 0;
        }

        public float GetCurrentProgress()
        {
            var eventId = AkSoundEngine.GetEventIDFromPlayingID(_playingId);

            if (eventId == 0)
            {
                _playingId = 0;
                _playingEvent = default;
                return 0;
            }
            
            var result = AkSoundEngine.GetSourcePlayPosition(_playingId, out var playingPosition);
            var playingPositionInSeconds = playingPosition / 1000f;
            var progress = playingPositionInSeconds / _playingEvent.maxDuration;
            return progress;
        }
    }
#endif
}