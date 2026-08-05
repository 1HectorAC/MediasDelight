
using MediasDelight.Web.Models;
using MediasDelight.Web.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediasDelight.Web.Services.Implementations;

public class MediaItemService : IMediaItemService
{
    private readonly IGenericRepository<MediaItem> _mediaItemRepo;

    public MediaItemService(IGenericRepository<MediaItem> mediaItemRepo)
    {
        _mediaItemRepo = mediaItemRepo;
    }

    public async Task<List<MediaItem>> GetAllAsync()
    {
        var items = new List<MediaItem>
        {
            new MediaItem {Id=1, UserId="1", MediaTypeId=1, Name="Awsome show", Rating=5, Description="goodish", TimeStamp= DateTime.UtcNow},
            new MediaItem {Id=2, UserId="1", MediaTypeId=1, Name="The thing", Rating=5, Description="goodish", TimeStamp= DateTime.UtcNow}
        };
        return items;
    }

    public async Task<List<MediaItem>> GetAllByUserIdAsync(string userId)
    {
        var mediaItems = await _mediaItemRepo
            .Query()
            .Where(i => i.UserId == userId)
            .ToListAsync();

        return mediaItems;
    }

    public async Task<List<MediaItem>> GetAllByUserIdAndMediaTypeIdAsync(string userId, int mediaTypeId)
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
        await _mediaItemRepo.AddAsync(mediaItem);
        await _mediaItemRepo.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var mediaItem = await _mediaItemRepo.GetByIdAsync(id) ?? throw new Exception();

        _mediaItemRepo.Remove(mediaItem);
        await _mediaItemRepo.SaveChangesAsync();
    }
}