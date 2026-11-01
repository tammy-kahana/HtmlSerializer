using System.Text.RegularExpressions;

namespace HtmlSerializer
{
    public static class HtmlParser
    {
        private static readonly HttpClient _client = new();

        public static async Task<string> Load(string url) => await _client.GetStringAsync(url);

        public static HtmlElement BuildTree(string html)
        {
            var root = new HtmlElement { Name = "root" };
            var current = root;
            var helper = HtmlHelper.Instance;

            foreach (var part in Regex.Split(Regex.Replace(html, @"\s+", " "), "(<[^>]*>)"))
            {
                var line = part.Trim();
                if (line == "") continue;

                if (!line.StartsWith("<"))
                {
                    current.InnerHtml = (current.InnerHtml + " " + line).Trim();
                    continue;
                }

                line = line.Trim('<', '>').Trim();
                var tagName = line.Split(' ')[0].TrimEnd('/').ToLower();

                if (tagName == "/html") break;
                if (tagName.StartsWith("/")) { current = current.Parent ?? current; continue; }
                if (!helper.HtmlTags.Contains(tagName)) continue;

                var element = new HtmlElement { Name = tagName, Parent = current };
                current.Children.Add(element);

                foreach (Match m in Regex.Matches(line[tagName.Length..], @"([^\s=]+)=(?:""([^""]*)""|'([^']*)'|([^\s""']+))"))
                    element.Attributes[m.Groups[1].Value.ToLower()] = m.Groups[2].Value + m.Groups[3].Value + m.Groups[4].Value;

                element.Id = element.Attributes.GetValueOrDefault("id");
                element.Classes = element.Attributes.GetValueOrDefault("class", "")
                                         .Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

                if (!line.EndsWith("/") && !helper.HtmlVoidTags.Contains(tagName))
                    current = element;
            }

            return root;
        }
    }
}
