using Microsoft.AspNetCore.Mvc;
using SearchRankChecker.Api.Models;
using SearchRankChecker.Api.Services;

namespace SearchRankChecker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }

    [HttpPost]
    public async Task<ActionResult<SearchResponse>> SearchAsync([FromBody] SearchRequest request)
    {
        // Ensure keyword and URL are both provided
        if (string.IsNullOrWhiteSpace(request.Keyword) || string.IsNullOrWhiteSpace(request.Url))
            return BadRequest(new { error = "Keyword and URL must be provided." });

        try
        {
            var positions = await _searchService.GetRankPositionsAsync(request.Keyword, request.Url);
            return Ok(new SearchResponse { Positions = positions });
        }
        catch (InvalidOperationException ex)
        {
            // This is likely due to Google blocking or redirecting the request
            return StatusCode(429, new { error = ex.Message });
        }
        catch (Exception ex)
        {
            // Any other unexpected failure
            return StatusCode(500, new { error = "Something went wrong. Please try again later." });
        }
    }
}