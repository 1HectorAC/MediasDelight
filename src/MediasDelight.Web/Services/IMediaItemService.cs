
using MediasDelight.Web.Models;

namespace MediasDelight.Web.Services;

public interface IMediaItemService
{
    Task<List<MediaItem>> GetAllAsync();

    Task<List<MediaItem>> GetAllByMediaTypeIdAsync(int mediaTypeId);

    Task AddAsync(MediaItem mediaItem);


    
}