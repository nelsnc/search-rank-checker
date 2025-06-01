using SearchRankChecker.Api.Parsers;
using SearchRankChecker.Tests.TestHelpers;

namespace SearchRankChecker.Tests;

/// <summary>
/// Unit tests for the GoogleSearchResultParser.
/// These tests use real saved HTML pages and validate that the parser correctly finds the rank positions
/// of a target URL (e.g., "infotrack.co.uk") in different search result scenarios.
/// </summary>
public class GoogleSearchResultParserTests
{
    private readonly GoogleSearchResultParser _parser = new();

    #region Test Constants

    private const string InfotrackUrl = "infotrack.co.uk";
    private const string InfotrackFile = "google_infotrack.html";
    private const string LandRegistryFile = "google_land_registry_search.html";
    private const string MissingUrl = "this-should-not-exist.com";

    #endregion

    [Fact]
    public void Parser_CorrectlyFindsInfotrackPosition_ForLandRegistrySearch()
    {
        var html = HtmlLoader.Load(LandRegistryFile);

        var positions = _parser.Parse(html, InfotrackUrl);

        Assert.Equal(new List<int> { 17 }, positions);
    }

    [Fact]
    public void Parser_CorrectlyFindsInfotrackPositions_ForInfotrackSearch()
    {
        var html = HtmlLoader.Load(InfotrackFile);

        var positions = _parser.Parse(html, InfotrackUrl);

        Assert.Equal(new List<int> { 1, 16 }, positions);
    }

    [Fact]
    public void Parser_ReturnsZero_WhenUrlNotPresent()
    {
        var html = HtmlLoader.Load(LandRegistryFile);

        var positions = _parser.Parse(html, MissingUrl);

        Assert.Single(positions);
        Assert.Equal(0, positions.First());
    }
}
