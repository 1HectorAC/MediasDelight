
using System.ComponentModel.DataAnnotations;

namespace MediasDelight.Web.Models.ViewModels;

public class CreateMediaItemViewModel
{

    [Required]
    public int MediaTypeId {get; set;}

    [Required]
    [StringLength(128)]
    public required string Name {get ; set;}

    [Required]
    [Range(0,10)]
    public int Rating {get; set;}

    [StringLength(500)]
    public string? Description {get; set;}

    [StringLength(500)]
    public string? Likes {get; set;}

    [StringLength(500)]
    public string? Dislikes {get; set;}


}