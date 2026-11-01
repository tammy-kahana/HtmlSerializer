using HtmlSerializer;

var html = await HtmlSerializer.HtmlSerializer.Load("https://maft.co.il");
var root = HtmlSerializer.HtmlSerializer.BuildTree(html);

var results = root.Query(Selector.Parse("div a"));
Console.WriteLine(results.Count);

var first = results.FirstOrDefault();
if (first != null)
    Console.WriteLine(string.Join(" < ", first.Ancestors().Select(a => a.Name)));
