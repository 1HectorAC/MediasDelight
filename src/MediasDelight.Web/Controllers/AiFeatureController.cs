
using MediasDelight.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.SolutionPersistence.Model;

namespace MediasDelight.Web.Controllers;

public class AiFeatureController: Controller
{
    private readonly GeminiService _geminiService;

    public AiFeatureController(GeminiService geminiService)
    {
        _geminiService = geminiService;
    }
    
    public async Task<IActionResult> AnalyzeMe()
    {
        return View();
    }

    [HttpPost]
    public async Task<string> GenerateResponse()
    {
        try
        {
            string test = "Describe what a giraffe looks like";
            var result = await _geminiService.GenerateTextAsync(test);
            return result;
        }
        catch(Exception ex)
        {
            return "API call error.";
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