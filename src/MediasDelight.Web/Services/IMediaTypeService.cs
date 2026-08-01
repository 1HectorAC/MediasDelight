
using MediasDelight.Web.Models;

namespace MediasDelight.Web.Services;

public interface IMediaTypeService
{
    Task<List<MediaType>> GetAllAsync();

    
}