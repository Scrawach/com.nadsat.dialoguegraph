using System.Collections.Generic;

namespace Editor.Audios
{
    public class AudioEventsProvider
    {
        public IEnumerable<string> GetEvents()
        {
            foreach (var eventUnit in AkWwiseProjectInfo.GetData().EventWwu)
            foreach (var e in eventUnit.List)
            {
                yield return e.Name;
            }
        }
    }
}