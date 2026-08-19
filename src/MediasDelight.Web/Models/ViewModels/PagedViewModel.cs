
namespace MediasDelight.Web.Models.ViewModels;

public class PagedViewModel<T>
{
    public List<T> Items { get; set; } = new List<T> { };

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalItems { get; set; }

    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public string SearchTerm { get; set; } = "";

    public string TypeFilter { get; set; } = "";

    public int MinRatingFilter { get; set; } = 0;

    public int MaxRatingFilter { get; set; } = 0;
}