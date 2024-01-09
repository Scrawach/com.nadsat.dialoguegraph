using System.Collections.Generic;
using System.Linq;

namespace Editor
{
    public class PhraseRepository
    {
        private readonly Dictionary<string, string> _content = new Dictionary<string, string>()
        {
            ["tutor_elena_001"] = "911, оператор службы спасения Елена. Чем могу помочь?",
            ["tutor_elena_002"] = "Здравствуйте, мистер Уильямс. Так точно.",
            ["tutor_mark_001"] = "Приветствую, Елена! Это начальство беспокоит. Вы уже на рабочем месте?",
            ["tutor_mark_002"] = "Дальше просто Марк, пожалуйста. Вы пришли раньше, чем нужно, взяли трубку сразу... Может, вам ещё и кофе здешний нравится? Ладно, шучу — его никто не любит.",
        };

        public string[] AllKeys() =>
            _content.Keys.ToArray();

        public string Find(string key) =>
            _content[key];
    }
}