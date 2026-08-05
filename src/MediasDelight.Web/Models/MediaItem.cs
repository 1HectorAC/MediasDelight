
using System.ComponentModel.DataAnnotations;

namespace MediasDelight.Web.Models;

public class MediaItem
{
    public int Id { get; set;}

    [Required]
    public required string UserId {get; set;}

    public ApplicationUser? User {get; set;}

    public int MediaTypeId {get; set;}

    public MediaType? MediaType {get; set;}

    [Required]
    [StringLength(128)]
    public required string Name {get; set;}

    [Required]
    [Range(0, 10)]
    public int Rating {get; set;}

    [StringLength(500)]
    public string? Description {get; set;}

    [StringLength(500)]
    public string? Likes {get; set;}

    [StringLength(500)]
    public string? Dislikes {get; set;}

    [Required]
    public DateTime TimeStamp {get; set;}


    [Range(0, 3000)]
    public string? YearViewed {get; set;}


}