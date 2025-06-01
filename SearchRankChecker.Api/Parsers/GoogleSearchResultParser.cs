using System.Text.RegularExpressions;

namespace SearchRankChecker.Api.Parsers;

public class GoogleSearchResultParser : ISearchResultParser
{
    // Parse html and get the positions of the target url
    public List<int> Parse(string html, string url)
    {
        var positions = new List<int>();

        var blocks = Regex.Matches(html, @"<div class=\""MjjYud\"">(.*?)</div>\s*</div>", RegexOptions.Singleline);
        int position = 1;

        foreach (Match block in blocks)
        {
            var blockHtml = block.Groups[1].Value;

            // Skip blocks that don’t have a standard result headline or anchor like the "People also ask" section
            if (!blockHtml.Contains("href=\"", StringComparison.OrdinalIgnoreCase) ||
                !blockHtml.Contains("<h3", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var match = Regex.Match(blockHtml, @"href=\""([^\""]+)\""", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                var href = match.Groups[1].Value;

                if (href.Contains(url, StringComparison.OrdinalIgnoreCase))
                {
                    positions.Add(position);
                }
            }

            position++;
        }

        return positions.Count > 0 ? positions : new List<int> { 0 };
    }
}