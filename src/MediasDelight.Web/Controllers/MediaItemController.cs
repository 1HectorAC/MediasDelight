
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
        var mediaItems = await _service.GetAllAsync();
        var mediaTypes = await _mediaTypeService.GetAllAsync();
        var vm = new MediaItemIndexPageViewModel
        {
            Items = mediaItems,
            AddItem = new CreateMediaItemViewModel {Name = string.Empty},
            MediaTypes = new SelectList(mediaTypes, "Id", "Name")
        };

        return View(vm);
    }


    [HttpPost]
    public async Task<IActionResult> Add(CreateMediaItemViewModel addItem)
    {
        if (!ModelState.IsValid)
        {
            var vm = new MediaItemIndexPageViewModel
            {
                Items = await _service.GetAllAsync(),
                AddItem = addItem,
                MediaTypes = new SelectList(await _mediaTypeService.GetAllAsync(), "Id", "Name")
            };

            return View("Index", vm);
        }
        var mediaItem = new MediaItem
        {
            Name = addItem.Name,
            UserId = "get user Id from logged in user",
            MediaTypeId = addItem.MediaTypeId,
            Rating = addItem.Rating,
            Description = addItem.Description,
            TimeStamp = DateTime.UtcNow
        };

        await _service.AddAsync(mediaItem);

        return RedirectToAction("Index");
    }

    //Maybe Remove later, with view
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