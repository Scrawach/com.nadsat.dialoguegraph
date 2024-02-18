using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Editor.Audios
{
    public class AudioEditorService
    {
        private const string PathToInitializer = "WwiseGlobalEditor";
        private static readonly List<AkBankManagerAsync.AsyncLoadingBankResult> _loadedBanks = new();

        private AkInitializer _initializer;
        private uint _playingId;

        public async void Initialize()
        {
            _initializer = Resources.Load<AkInitializer>("WwiseGlobalEditor");
            AkSoundEngineController.Instance.Init(_initializer);
            
            if (!IsBankLoaded("Main")) 
                await LoadBank("Main");
        }
        
        private bool IsBankLoaded(string bankName) =>
            _loadedBanks.Any(bank => bank.Name == bankName);

        private async Task LoadBank(string bankName)
        {
            var result = await AkBankManagerAsync.LoadBankAsync(bankName);
            _loadedBanks.Add(result);
            Debug.Log($"Load bank {bankName} with result {result.Result}");
        }
        
        public void Play(string eventName)
        {
            _playingId = AkSoundEngine.PostEvent(eventName, _initializer.gameObject);
        }

        public void Stop(string eventName)
        {
            Debug.Log($"stop {eventName}");
            AkSoundEngine.StopPlayingID(_playingId);
        }

        public void Seek(string eventName, float progress)
        {
            Debug.Log($"seek {eventName} to {progress}");
            AkSoundEngine.SeekOnEvent(eventName, _initializer.gameObject, progress);
        }

        public void StopAll()
        {
            Debug.Log($"stop all");
            AkSoundEngine.StopAll();
        }
    }
}