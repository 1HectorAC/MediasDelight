
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
    public async Task<IActionResult> Index(int page = 1, int pageSize = 3, string searchTerm = "", int? minRatingFilter = null, int? maxRatingFilter = null, int? typeIdFilter = null)
    {

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return RedirectToAction("Index", "Home");

        // Validation: check minRatingFilter is less than MaxRating Filter
        if (minRatingFilter != 0 && maxRatingFilter != 0 && maxRatingFilter < minRatingFilter)
            minRatingFilter = maxRatingFilter;
     
        var mediaItems = await _service.GetAllByUserIdAsync(userId);

        // Formating: Apply filters
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            mediaItems = mediaItems.Where(i => i.Name.ToLower() == searchTerm.ToLower()).ToList();
        }
        if(typeIdFilter != null && typeIdFilter != 0)
        {
            mediaItems = mediaItems.Where(i => i.MediaTypeId == typeIdFilter).ToList();
        }
        if(minRatingFilter != null && minRatingFilter != 0)
        {
            mediaItems = mediaItems.Where(i => i.Rating >= minRatingFilter).ToList();
        }
        if(maxRatingFilter != null && maxRatingFilter != 0)
        {
            mediaItems = mediaItems.Where(i => i.Rating <= maxRatingFilter).ToList();
        }
        
        var totalItems = mediaItems.Count;

        var mediaItemsPage = mediaItems.Skip((page - 1) * pageSize).Take(pageSize);
        var mediaItemsVm = mediaItemsPage.Select(i => new MediaItemViewModel
        {
            Id = i.Id,
            MediaTypeName = i.MediaType?.Name ?? "Empty Value",
            Name = i.Name,
            Rating = i.Rating,
            Likes = i.Likes,
            Dislikes = i.Dislikes
        }).ToList();
        var pagedViewModel = new PagedViewModel<MediaItemViewModel>
        {
            Items = mediaItemsVm,
            PageNumber = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            SearchTerm = searchTerm,
            TypeIdFilter = typeIdFilter ?? 0,
            MinRatingFilter = minRatingFilter ?? 0,
            MaxRatingFilter = maxRatingFilter ?? 0
        };

        // Data Retrive: MediaTypes, for options selection in view.
        var mediaTypes = await _mediaTypeService.GetAllAsync();
        ViewBag.mediaTypes = new SelectList(mediaTypes, "Id", "Name");
        if(typeIdFilter != null && typeIdFilter != 0){
            ViewBag.mediaTypeName = mediaTypes.FirstOrDefault(i => i.Id == typeIdFilter)?.Name;
        }
        
        Console.WriteLine("Filters: " + typeIdFilter + " " + minRatingFilter + " " + maxRatingFilter);
        return View(pagedViewModel);
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

        // Validation: check user owns the mediaItem
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return RedirectToAction("Index", "Home");
        if(userId != mediaItem.UserId)
            return RedirectToAction("Index");

        try
        {
            await _service.UpdateAsync(mediaItem);
        }
        catch
        {
            return RedirectToAction("Index");
        }

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