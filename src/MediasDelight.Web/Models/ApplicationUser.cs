
using Microsoft.AspNetCore.Identity;

namespace MediasDelight.Web.Models;

public class ApplicationUser: IdentityUser
{
    public ICollection<MediaItem> MediaItems {get; set;} = [];

}