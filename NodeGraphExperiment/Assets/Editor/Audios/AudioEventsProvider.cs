using System.Collections.Generic;

namespace Editor.Audios
{
    public class AudioEventsProvider
    {
        public IEnumerable<string> GetEvents()
        {
            for (var i = 0; i < 3; i++)
            {
                yield return $"Audio Event #{i}";
            }
        }
    }
}