
using MediasDelight.Web.Models;
using MediasDelight.Web.Models.ViewModels;
using MediasDelight.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace MediasDelight.Web.Controllers;

public class MediaItemController: Controller
{
    private readonly IMediaItemService _service;

    public MediaItemController(IMediaItemService service)
    {
        _service = service;
    }
    public async Task<IActionResult> Index()
    {
        var items = await _service.GetAllAsync();
        return View(items);
    }

    public async Task<IActionResult> Create()
    {
        List<MediaType> list = [new MediaType{Id = 1, Name= "Show"},new MediaType{Id = 2, Name= "Movie"},new MediaType{Id = 3, Name= "videoGame"}];
        ViewBag.mediaTypes = new SelectList(list, "Id", "Name");
        return View();

    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMediaItemViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            List<MediaType> list = [new MediaType{Id = 1, Name= "Show"},new MediaType{Id = 2, Name= "Movie"},new MediaType{Id = 3, Name= "videoGame"}];
            ViewBag.mediaTypes = new SelectList(list, "Id", "Name");
            return View(viewModel);
        }
        Console.WriteLine($"Rating: {viewModel.MediaTypeId}");


        return RedirectToAction("Index");
    }


}