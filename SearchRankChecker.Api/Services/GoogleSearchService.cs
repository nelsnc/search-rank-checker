using SearchRankChecker.Api.Parsers;

namespace SearchRankChecker.Api.Services;

public class GoogleSearchService : ISearchService
{
    #region Fields

    private readonly HttpClient _httpClient;
    private readonly ISearchResultParser _parser;

    #endregion

    #region Constructor

    public GoogleSearchService(HttpClient httpClient, ISearchResultParser parser)
    {
        _httpClient = httpClient;
        _parser = parser;
    }

    #endregion

    #region Public Methods

    public async Task<List<int>> GetRankPositionsAsync(string keyword, string url)
    {
        var encodedQuery = Uri.EscapeDataString(keyword);
        var searchUrl = $"https://www.google.co.uk/search?num=100&q={encodedQuery}";

        var request = new HttpRequestMessage(HttpMethod.Get, searchUrl);
        request.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 Chrome/90.0 Safari/537.36");

        var response = await _httpClient.SendAsync(request);

        // Throw an exception if the HTTP response indicates an error
        if (!response.IsSuccessStatusCode)
        {
            string reason = response.ReasonPhrase ?? "Unknown error";
            string statusInfo = $"Status {(int)response.StatusCode} - {reason}";
            throw new InvalidOperationException($"Google returned an error: {statusInfo}");
        }

        var html = await response.Content.ReadAsStringAsync();

        // Check if Google blocked the response by inspecting the HTML structure
        if (_IsBlockedHtml(html))
        {
            throw new InvalidOperationException("Google returned an unusable search page. The request was likely blocked.");
        }

        return _parser.Parse(html, url);
    }

    #endregion

    #region Private Helpers

    private bool _IsBlockedHtml(string html)
    {
        return html.Contains("Please click <a href=\"/httpservice/retry/enablejs?", StringComparison.OrdinalIgnoreCase) ||
               !html.Contains("MjjYud", StringComparison.OrdinalIgnoreCase);
    }

    #endregion
}