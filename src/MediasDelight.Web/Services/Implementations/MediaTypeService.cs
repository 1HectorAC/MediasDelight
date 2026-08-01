
using MediasDelight.Web.Models;

namespace MediasDelight.Web.Services.Implementations;

public class MediaTypeService : IMediaTypeService
{
    public async Task<List<MediaType>> GetAllAsync()
    {
        // Change to Db query later.
        List<MediaType> list = [
            new MediaType{Id = 1, Name= "Show"},
            new MediaType{Id = 2, Name= "Movie"},
            new MediaType{Id = 3, Name= "videoGame"}
            ];

        return list;
    }

}