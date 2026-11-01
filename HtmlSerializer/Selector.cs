using System.Text.RegularExpressions;

namespace HtmlSerializer
{
    public class Selector
    {
        public string? TagName { get; set; }
        public string? Id { get; set; }
        public List<string> Classes { get; set; } = new();

        public Selector? Parent { get; set; }
        public Selector? Child { get; set; }

        public static Selector Parse(string query)
        {
            Selector? root = null, current = null;

            foreach (var level in query.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                var selector = new Selector { Parent = current };
                if (current == null) root = selector; else current.Child = selector;
                current = selector;

                foreach (var part in Regex.Split(level, "(?=[#.])").Where(p => p != ""))
                {
                    if (part.StartsWith("#")) current.Id = part[1..];
                    else if (part.StartsWith(".")) current.Classes.Add(part[1..]);
                    else if (HtmlHelper.Instance.HtmlTags.Contains(part.ToLower())) current.TagName = part.ToLower();
                }
            }

            return root ?? new Selector();
        }
    }
}
