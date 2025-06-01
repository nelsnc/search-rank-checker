namespace SearchRankChecker.Api.Parsers;

public interface ISearchResultParser
{
    List<int> Parse(string html, string url);
}