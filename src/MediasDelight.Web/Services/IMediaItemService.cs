
using MediasDelight.Web.Models;

namespace MediasDelight.Web.Services;

public interface IMediaItemService
{

    Task<List<MediaItem>> GetAllByUserIdAsync(string userId);

    Task<List<MediaItem>> GetAllByUserIdAndMediaTypeIdAsync(string userId, int mediaTypeId);

    Task<MediaItem> GetByIdAsync(int id);
    
    Task AddAsync(MediaItem mediaItem);

    Task DeleteAsync(int id);

}