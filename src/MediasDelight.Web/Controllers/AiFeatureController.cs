
using Microsoft.AspNetCore.Mvc;

namespace MediasDelight.Web.Controllers;

public class AiFeatureController: Controller
{
    
    
    public async Task<IActionResult> AnalyzeMe()
    {
        return View();
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