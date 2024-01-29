using System;

namespace Editor.AssetManagement
{
    public class LanguageProvider
    {
        public string CurrentLanguage { get; private set; }

        public event Action<string> LanguageChanged;

        public void ChangeLanguage(string targetLanguage)
        {
            CurrentLanguage = targetLanguage;
            LanguageChanged?.Invoke(targetLanguage);
        }

        public string[] AllLanguages() =>
            new[] {"Russian", "English"};
    }
    
}