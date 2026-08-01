
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MediasDelight.Web.Models.ViewModels;

public class MediaItemIndexPageViewModel
{
    [Required]
    public required  List<MediaItem> Items {get; set;}

    [Required]
    public required CreateMediaItemViewModel AddItem {get; set;}

    [Required]
    public required SelectList MediaTypes {get; set;}
}