using System.Collections.Generic;
using System.Linq;

namespace Editor.Localization
{
    public class LocalizedTable
    {
        private readonly CsvAsset _asset;

        public LocalizedTable(CsvAsset asset) =>
            _asset = asset;

        public IEnumerable<(string key, LocalizedString text)> All()
        {
            foreach (var row in _asset.Rows().Skip(1))
            {
                var localizedString = new LocalizedString();
                localizedString.Text["Russian"] = row[1];
                yield return (row[0], localizedString);
            }
        }
    }
}