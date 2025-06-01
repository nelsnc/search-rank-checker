namespace SearchRankChecker.Api.Services;

public interface ISearchService
{
    Task<List<int>> GetRankPositionsAsync(string keyword, string url);
}