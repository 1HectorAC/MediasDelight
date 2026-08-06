
using System.Security.Claims;
using MediasDelight.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        // DATA RETRIVE: Get mediaType of passed in mediaTypeId
        var mediaType = await _mediaTypeService.GetByIdAsync(mediaTypeId);
        if (mediaType is null)
            return BadRequest("mediaType Id passed in was not valid");

        // DATA RETRIVE: Get users mediaItems
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return RedirectToAction("Index", "Home");
        var mediaItems = await _mediaItemService.GetAllByUserIdAndMediaTypeIdAsync(userId, mediaTypeId);

        // VALIDATION: user has no mediaItems handling
        if(mediaItems.Count == 0)
        {
            return Json(new { response = "No items exits in Media Items" });
        }

        // DATA FORMATING: Construct prompt for api call
        string mediaTypeSentence = $"The media type is {mediaType.Name}.";

        string[] mediaItemStrings = mediaItems.Select(i => $"[{i.Name}::{i.Rating}::{i.Likes}::{i.Dislikes}]").ToArray();
        string mediaItemFullString = string.Join(",", mediaItemStrings);
        string mediaItemSentece = "The media items are format as follow: [title::rating::Likes::Dislikes],[title2::rating2::Likes2::Dislikes2],… and so on. Here are the media items: " + mediaItemFullString;

        // ISSUE: Handle items not being recognized. Add to prompt.
        // ISSUE: Add more response text formating to improve readability.
        // ISSUE: Change return formating (respond, list of items not recognize). Add to prompt
        string prompt = "Your goal is the help users better understand themselves and their media preferences by analyzing some data you are given. You will be given a media type and a list of media items with some information (Likes, Dislikes, rating out of 10). You will analyze them and respond with an analysis of their preference." + mediaTypeSentence + mediaItemSentece;

        try
        {
            var result = await _geminiService.GenerateTextAsync(prompt);
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