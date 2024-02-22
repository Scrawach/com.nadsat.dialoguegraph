using System.Collections.Generic;
using System.Linq;

namespace Runtime.Localization
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
                for (var i = 0; i < headers.Length; i++)
                    localizedString.Add(headers[i], row[i]);
                yield return (row[0], localizedString);
            }
        }

        public string[] GetAvailableLanguages() =>
            _languages;

        private static string[] ParseLanguages(string[] headers)
        {
            var result = new List<string>();

            for (var i = 1; i < headers.Length; i++)
                result.Add(headers[i]);

            return result.ToArray();
        }
    }
}