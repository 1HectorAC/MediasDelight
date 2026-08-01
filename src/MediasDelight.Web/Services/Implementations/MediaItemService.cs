
using MediasDelight.Web.Models;

namespace MediasDelight.Web.Services.Implementations;

public class MediaItemService: IMediaItemService
{
    public async Task<List<MediaItem>> GetAllAsync()
    {
        var items = new List<MediaItem>
        {
            new MediaItem {Id=1, UserId="1", MediaTypeId=1, Name="Awsome show", Rating=5, Description="goodish", TimeStamp= DateTime.UtcNow},
            new MediaItem {Id=2, UserId="1", MediaTypeId=1, Name="The thing", Rating=5, Description="goodish", TimeStamp= DateTime.UtcNow}

        };
        return items;
    }

    public async Task AddAsync(MediaItem mediaItem)
    {
        
    }
}