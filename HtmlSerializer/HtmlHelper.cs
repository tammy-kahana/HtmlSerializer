using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace HtmlSerializer
{
    public class HtmlHelper
    {
        private static readonly HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;

        public string[] HtmlTags { get; }
        public string[] HtmlVoidTags { get; }

        private HtmlHelper()
        {
            var tagsJson = File.ReadAllText("HtmlTags.json");
            HtmlTags = JsonSerializer.Deserialize<string[]>(tagsJson) ?? Array.Empty<string>();

            var voidTagsJson = File.ReadAllText("HtmlVoidTags.json");
            HtmlVoidTags = JsonSerializer.Deserialize<string[]>(voidTagsJson) ?? Array.Empty<string>();
        }
    }
}
