namespace HtmlSerializer
{
    public static class HtmlElementExtensions
    {
        public static HashSet<HtmlElement> Query(this HtmlElement element, Selector selector)
        {
            var result = new HashSet<HtmlElement>();
            Find(element, selector, result);
            return result;
        }

        private static void Find(HtmlElement element, Selector selector, HashSet<HtmlElement> result)
        {
            var matches = element.Descendants().Skip(1).Where(e => e.Matches(selector));

            if (selector.Child == null)
                result.UnionWith(matches);
            else
                foreach (var match in matches)
                    Find(match, selector.Child, result);
        }

        private static bool Matches(this HtmlElement e, Selector s) =>
            (s.TagName == null || e.Name == s.TagName) &&
            (s.Id == null || e.Id == s.Id) &&
            s.Classes.All(e.Classes.Contains);
    }
}
