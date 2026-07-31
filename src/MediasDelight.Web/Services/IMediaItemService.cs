
using MediasDelight.Web.Models;

namespace MediasDelight.Web.Services;

public interface IMediaItemService
{
    Task<List<MediaItem>> GetAllAsync();

    Task AddAsync();

    
}