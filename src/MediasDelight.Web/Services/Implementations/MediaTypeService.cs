
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
        var mediaTypes = await _mediaTypeRepo
            .Query()
            .AsNoTracking()
            .ToListAsync();

        return mediaTypes;
    }

    public async Task<MediaType?> GetByIdAsync(int id)
    {
        var mediaType = await _mediaTypeRepo.GetByIdAsync(id);
        return mediaType;
    }

}