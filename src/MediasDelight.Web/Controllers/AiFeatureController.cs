
using System.Security.Claims;
using MediasDelight.Web.Services;
using MediasDelight.Web.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.SolutionPersistence.Model;

namespace MediasDelight.Web.Controllers;

[Authorize]
public class AiFeatureController : Controller
{
    private readonly GeminiService _geminiService;
    private readonly IMediaTypeService _mediaTypeService;

    private readonly IMediaItemService _mediaItemService;

    public AiFeatureController(
        GeminiService geminiService,
         IMediaTypeService mediaTypeService,
         IMediaItemService mediaItemService
         )
    {
        _geminiService = geminiService;
        _mediaTypeService = mediaTypeService;
        _mediaItemService = mediaItemService;
    }

    public async Task<IActionResult> AnalyzeMe()
    {
        var mediaTypes = await _mediaTypeService.GetAllAsync();

        return View(mediaTypes);
    }

    [HttpPost]
    public async Task<IActionResult> GenerateAnalyzeMeResponse([FromBody] int mediaTypeId)
    {

        if (!ModelState.IsValid)
            return BadRequest("ModelType Id wasnt valid: " + mediaTypeId);

        // Data Retrive: Get mediaType of passed in mediaTypeId
        var mediaType = _mediaTypeService.GetByIdAsync(mediaTypeId);
        if (mediaType is null)
            return BadRequest("mediaType Id passed in was not valid");

        // Data Retrive: Get users mediaItems
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(userId is null)
            return RedirectToAction("Index", "Home");
        var mediaItems = await _mediaItemService.GetAllByUserIdAndMediaTypeIdAsync(userId, mediaTypeId);

        // Construct prompt for api call using MediaItems + mediaType

        try
        {
            // Test of Api call, remove later
            // test = "Describe what a giraffe looks like";
            //var result = await _geminiService.GenerateTextAsync(test);

            var result = "You have too much free time. Touch grass instead.";
            return Json(new { response = result });
        }
        catch (Exception)
        {
            return BadRequest("API response error.");
        }
    }

    public async Task<IActionResult> AssessWork()
    {
        return View();
    }
    public async Task<IActionResult> RecommendWorks()
    {
        return View();
    }

}