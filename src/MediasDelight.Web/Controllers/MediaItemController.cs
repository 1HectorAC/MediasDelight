
using MediasDelight.Web.Services;
using MediasDelight.Web.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

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


}