
using MediasDelight.Web.Models;
using MediasDelight.Web.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediasDelight.Web.Services.Implementations;

public class MediaTypeService : IMediaTypeService
{
    private readonly IGenericRepository<MediaType> _mediaTypeRepo;
    public MediaTypeService(IGenericRepository<MediaType> mediaTypeRepo)
    {
        _mediaTypeRepo = mediaTypeRepo;
    }

    public async Task<List<MediaType>> GetAllAsync()
    {
        var mediaTypes = await _mediaTypeRepo.Query().ToListAsync();

        return mediaTypes;
    }



    public async Task<MediaType?> GetByIdAsync(int id)
    {
        MediaType mediaType = new MediaType {Id = 1, Name="Video Game"};
        return mediaType;
    }

}