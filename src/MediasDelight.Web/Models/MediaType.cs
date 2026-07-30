using System.ComponentModel.DataAnnotations;

namespace MediasDelight.Web.Models;

public class MediaType
{
    public int Id {get; set;}

    [Required]
    [StringLength(128)]
    public required string Name {get; set;}

    public ICollection<MediaItem> MediaItems {get; set;} = [];
}