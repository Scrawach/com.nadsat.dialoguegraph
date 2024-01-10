using System.Collections.Generic;
using System.Linq;

namespace Editor.Localization
{
    public class LocalizedTable
    {
        private readonly CsvAsset _asset;
        private string[] _languages;

        public LocalizedTable(CsvAsset asset) =>
            _asset = asset;

        public IEnumerable<(string key, LocalizedString text)> LoadLocalizedStrings()
        {
            var headers = _asset.Rows().First();
            _languages = ParseLanguages(headers);
            
            foreach (var row in _asset.Rows().Skip(1))
            {
                var localizedString = new LocalizedString();

                for (var i = 1; i < headers.Length; i++)
                {
                    if (headers[i].StartsWith('.'))
                        continue;

                    localizedString.Text[headers[i]] = row[i];
                }
                
                yield return (row[0], localizedString);
            }
        }

        public string[] AvailableLanguages() =>
            _languages;

        private string[] ParseLanguages(string[] headers)
        {
            var result = new List<string>();
            
            for (var i = 1; i < headers.Length; i++)
            {
                if (headers[i].StartsWith('.'))
                    continue;
                
                result.Add(headers[i]);
            }

            return result.ToArray();
        }
    }
}