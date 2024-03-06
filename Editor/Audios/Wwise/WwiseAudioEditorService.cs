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

        public WwiseAudioEditorService(IAudioEventsProvider audioEventsProvider) =>
            _audioEventsProvider = audioEventsProvider;

        public void Initialize()
        {
            _initializer = Resources.Load<AkInitializer>(PathToInitializer);
            AkSoundEngineController.Instance.Init(_initializer);
            AkSoundEngine.RegisterGameObj(_initializer.gameObject);
            AkSoundEngine.SetCurrentLanguage("Russian");
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

        public void PlayEvent(string eventName)
        {
            const uint flags = (uint) AkCallbackType.AK_EnableGetSourcePlayPosition;
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