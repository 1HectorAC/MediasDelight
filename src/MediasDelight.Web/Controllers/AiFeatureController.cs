
using MediasDelight.Web.Services;
using MediasDelight.Web.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.SolutionPersistence.Model;

namespace MediasDelight.Web.Controllers;

public class AiFeatureController: Controller
{
    private readonly GeminiService _geminiService;
    private readonly IMediaTypeService _mediaTypeService;

    public AiFeatureController(GeminiService geminiService, IMediaTypeService mediaTypeService)
    {
        _geminiService = geminiService;
        _mediaTypeService = mediaTypeService;
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
        {
            return BadRequest("ModelType Id wasnt valid: " + mediaTypeId);
        }

        

        Console.WriteLine("id: " + mediaTypeId);
        try
        {
            // Test of Api call, remove later
            // test = "Describe what a giraffe looks like";
            //var result = await _geminiService.GenerateTextAsync(test);

            var result = "You have too much free time. Touch grass instead.";
            return Json(new {response = result});
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