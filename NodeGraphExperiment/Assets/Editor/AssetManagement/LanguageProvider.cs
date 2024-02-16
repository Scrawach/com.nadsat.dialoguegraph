using System;
using System.Collections.Generic;

namespace Editor.AssetManagement
{
    public class LanguageProvider
    {
        private readonly List<string> _availableLanguages = new();

        public string CurrentLanguage { get; private set; }

        public event Action<string> LanguageChanged;

        public event Action Changed;

        public void ChangeLanguage(string targetLanguage)
        {
            CurrentLanguage = targetLanguage;
            LanguageChanged?.Invoke(targetLanguage);
        }

        public void AddLanguage(string language)
        {
            _availableLanguages.Add(language);
            Changed?.Invoke();
            ChangeLanguage(language);
        }

        public string[] AllLanguages() =>
            _availableLanguages.ToArray();
    }
}