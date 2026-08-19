
using System.Security.Claims;
using MediasDelight.Web.Models;
using MediasDelight.Web.Models.ViewModels;
using MediasDelight.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace MediasDelight.Web.Controllers;

[Authorize]
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

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return RedirectToAction("Index", "Home");

        // Formating: convert MediaItems to MediaItemsViewModel
        var mediaItems = await _service.GetAllByUserIdAsync(userId);
        var mediaItemsVm = mediaItems.Select(i => new MediaItemViewModel
        {
            Id = i.Id,
            MediaTypeName = i.MediaType?.Name ?? "Empty Value",
            Name = i.Name,
            Rating = i.Rating,
            Likes = i.Likes,
            Dislikes = i.Dislikes
        }).ToList();

        return View(mediaItemsVm);
    }

    public async Task<IActionResult> Create()
    {
        // Data Retrive: MediaTypes, for options selection in view.
        var mediaTypes = await _mediaTypeService.GetAllAsync();
        ViewBag.mediaTypes = new SelectList(mediaTypes, "Id", "Name");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMediaItemViewModel vm)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return RedirectToAction("Index", "Home");

        if (!ModelState.IsValid)
        {
            // Data Retrive: MediaTypes, for options selection in view.
            var mediaTypes = await _mediaTypeService.GetAllAsync();
            ViewBag.mediaTypes = new SelectList(mediaTypes, "Id", "Name");

            return View(vm);
        }

        // Format & Add: mediaItem to db
        var mediaItem = new MediaItem
        {
            Name = vm.Name,
            UserId = userId,
            MediaTypeId = vm.MediaTypeId,
            Rating = vm.Rating,
            Likes = vm.Likes,
            Dislikes = vm.Dislikes,
            TimeStamp = DateTime.UtcNow
        };
        await _service.AddAsync(mediaItem);

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return RedirectToAction("Index", "Home");

        try
        {
            var mediaItem = await _service.GetByIdAsync(id);
            if (mediaItem.UserId != userId)
                return RedirectToAction("Index");

            // Data Retrive: MediaTypes, for options selection in view.
            var mediaTypes = await _mediaTypeService.GetAllAsync();
            ViewBag.mediaTypes = new SelectList(mediaTypes, "Id", "Name");

            return View(mediaItem);
        }
        catch (Exception)
        {
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(MediaItem mediaItem)
    {
        if (!ModelState.IsValid)
        {
            // Data Retrive: MediaTypes, for options selection in view.
            var mediaTypes = await _mediaTypeService.GetAllAsync();
            ViewBag.mediaTypes = new SelectList(mediaTypes, "Id", "Name");


            return View(mediaItem);
        }

        // TODO: need actual edit functionality

        return RedirectToAction("Index");
    }


    [HttpPost]
    public async Task<IActionResult> Add(CreateMediaItemViewModel addItem)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return RedirectToAction("Index", "Home");

        if (!ModelState.IsValid)
        {
            // ModelState failed: need to setup view models again, like in Index function

            // Formating: convert MediaItems to MediaItemsViewModel
            var mediaItems = await _service.GetAllByUserIdAsync(userId);
            var mediaItemsVm = mediaItems.Select(i => new MediaItemViewModel
            {
                Id = i.Id,
                MediaTypeName = i.MediaType?.Name ?? "Empty Value",
                Name = i.Name,
                Rating = i.Rating,
                Likes = i.Likes,
                Dislikes = i.Dislikes
            }).ToList();

            // Data Retrive: MediaTypes, for options selection in view.
            var mediaTypes = await _mediaTypeService.GetAllAsync();

            // Formating: Main view model being returned to view
            var vm = new MediaItemIndexPageViewModel
            {
                Items = mediaItemsVm,
                AddItem = addItem,
                MediaTypes = new SelectList(mediaTypes, "Id", "Name")
            };

            return View("Index", vm);
        }

        // Format & Add: mediaItem to db
        var mediaItem = new MediaItem
        {
            Name = addItem.Name,
            UserId = userId,
            MediaTypeId = addItem.MediaTypeId,
            Rating = addItem.Rating,
            Likes = addItem.Likes,
            Dislikes = addItem.Dislikes,
            TimeStamp = DateTime.UtcNow
        };
        await _service.AddAsync(mediaItem);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        // Need check if owned by current owner.

        try
        {
            await _service.DeleteAsync(id);
        }
        catch (Exception)
        {
            return BadRequest("MediaItem Id did not exits");
        }

        return RedirectToAction("Index");
    }
}