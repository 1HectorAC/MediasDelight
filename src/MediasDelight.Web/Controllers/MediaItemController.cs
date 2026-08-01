
using MediasDelight.Web.Models;
using MediasDelight.Web.Models.ViewModels;
using MediasDelight.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace MediasDelight.Web.Controllers;

public class MediaItemController : Controller
{
    private readonly IMediaItemService _service;

    private readonly IMediaTypeService _mediaTypeService;

    public MediaItemController(IMediaItemService service, IMediaTypeService mediaTypeService)
    {
        _service = service;
        _mediaTypeService = mediaTypeService;
    }
    public async Task<IActionResult> Index()
    {
        var items = await _service.GetAllAsync();
        return View(items);
    }

    public async Task<IActionResult> Create()
    {
        var list = await _mediaTypeService.GetAllAsync();
        ViewBag.mediaTypes = new SelectList(list, "Id", "Name");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMediaItemViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            var list = await _mediaTypeService.GetAllAsync();
            ViewBag.mediaTypes = new SelectList(list, "Id", "Name");
            return View(viewModel);
        }
        Console.WriteLine($"Rating: {viewModel.MediaTypeId}");

        return RedirectToAction("Index");
    }


}