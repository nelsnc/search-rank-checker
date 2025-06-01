namespace SearchRankChecker.Api.Models;

public class SearchRequest
{
    public string Keyword { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}