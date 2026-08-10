
using System.Security.Claims;
using MediasDelight.Web.DTO;
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
        if (mediaItems.Count == 0)
            return Json(new { response = "No items exits in Media Items" });

        // DATA FORMATING: Construct prompt for api call
        string mediaTypeSentence = $"The media type is {mediaType.Name}.";
        string[] mediaItemStrings = mediaItems.Select(i => $"[{i.Name}::{i.Rating}::{i.Likes}::{i.Dislikes}]").ToArray();
        string mediaItemFullString = string.Join(",", mediaItemStrings);
        string mediaItemSentece = "The media items are format as follow: [title::rating::Likes::Dislikes],[title2::rating2::Likes2::Dislikes2],… and so on. Here are the media items: " + mediaItemFullString;
        string setupSentence = "Your goal is the help users better understand themselves and their media preferences by analyzing some data you are given. You will be given a media type and a list of media items with some information (Likes, Dislikes, rating out of 10). You will analyze them and respond with an analysis of their preference. Here are some things to keep in mind. Try to be brief, meaningful and focus on describing the user in a couple paragraphs. Don’t add headlines or special characters to emphasis certain section. There is also no need to reference specific media Items in your response. ";
        string prompt = setupSentence + mediaTypeSentence + mediaItemSentece;

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
        var mediaTypes = await _mediaTypeService.GetAllAsync();

        return View(mediaTypes);
    }

    [HttpPost]
    public async Task<IActionResult> GenerateAssessWorkResponse([FromBody] AssessWorkRequestDto data)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid request body.");

        // DATA RETRIVE: Get mediaType of passed in mediaTypeId
        var mediaType = await _mediaTypeService.GetByIdAsync(data.MediaTypeId);
        if (mediaType is null)
            return BadRequest("mediaType Id passed in was not valid");

        // DATA RETRIVE: Get users mediaItems
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return RedirectToAction("Index", "Home");
        var mediaItems = await _mediaItemService.GetAllByUserIdAndMediaTypeIdAsync(userId, data.MediaTypeId);

        // VALIDATION: user has no mediaItems handling
        if (mediaItems.Count == 0)
            return Json(new { response = "No items exits in Media Items" });

        // DATA FORMATING: Construct prompt for api call
        string mediaTypeSentence = $"The media type is {mediaType.Name}.";
        string[] mediaItemStrings = mediaItems.Select(i => $"[{i.Name}::{i.Rating}::{i.Likes}::{i.Dislikes}]").ToArray();
        string mediaItemFullString = string.Join(",", mediaItemStrings);
        string mediaItemSentece = "The media items are format as follow: [title::rating::Likes::Dislikes],[title2::rating2::Likes2::Dislikes2],… and so on. Here are the media items: " + mediaItemFullString;
        string assessSentence = $"The media item that you are assessing is called {data.MediaWorkName}. ";
        string setupSentence = "Your goal is to assess whether the user will like or dislike a media item based on the information provided (a media type and a list of media items with some information including Likes, Dislikes, rating out of 10). You will try to analyze a given media item and respond with a rating out of 10 (0 being completely hated and 10 being the maximum enjoyment) about how much you think they will enjoy the work and a description of your reasoning. Try to be brief, meaningful and focus on describing your reasoning in a couple paragraphs. Don’t add headlines or special characters to emphasis certain section. There is also no need to reference specific media Items in your response. If you don’t recognized the media item that you will be assessing then forget everything else and just respond with \"Sorry, the media item you provided was not recognized.\". ";
        string prompt = setupSentence + assessSentence + mediaTypeSentence + mediaItemSentece;

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
    public async Task<IActionResult> RecommendWorks()
    {
        var mediaTypes = await _mediaTypeService.GetAllAsync();

        return View(mediaTypes);
    }

    [HttpPost]
    public async Task<IActionResult> GenerateRecommendWorksResponse([FromBody] int mediaTypeId)
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
        if (mediaItems.Count == 0)
            return Json(new { response = "No items exits in Media Items" });

        // DATA FORMATING: Construct prompt for api call
        string mediaTypeSentence = $"The media type is {mediaType.Name}.";
        string[] mediaItemStrings = mediaItems.Select(i => $"[{i.Name}::{i.Rating}::{i.Likes}::{i.Dislikes}]").ToArray();
        string mediaItemFullString = string.Join(",", mediaItemStrings);
        string mediaItemSentece = "The media items are format as follow: [title::rating::Likes::Dislikes],[title2::rating2::Likes2::Dislikes2],… and so on. Here are the media items: " + mediaItemFullString;
        string setupSentence = "Your goal is to provide recommended media works based on the information provided (a media type and a list of media items with some information including Likes, Dislikes, rating out of 10). You will try to analyze the media items provided and respond with 3-5 media works, based on the Media Type provided, that the user might like as well as a description of your reasoning for each.  Here are some things to keep in mind.  Try to be brief, meaningful and focus on describing the reasoning in a couple paragraphs. Don’t add headlines or special characters to emphasis certain section. There is also no need to reference specific media Items in your response. ";
        string prompt = setupSentence + mediaTypeSentence + mediaItemSentece;

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
}