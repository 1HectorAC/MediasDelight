
using MediasDelight.Web.Models;
using MediasDelight.Web.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace MediasDelight.Web.Tests.Repository;


public class GenericRepositoryTests
{
    [Fact]
    public async Task Query_WithMediaItems_ReturnQueryable()
    {
        // Arrange
        var repo = RepositoryHelper.Create<MediaItem>(out var context);

        await context.MediaItems.AddRangeAsync(
            new MediaItem {Id=1, MediaTypeId = 1, UserId = "1", Name="Stuff", Rating=5, TimeStamp = new DateTime(2025, 5,5)},
            new MediaItem {Id=2, MediaTypeId = 1, UserId = "1", Name="Stuff", Rating=5, TimeStamp = new DateTime(2025, 5,5)}
        );
        await context.SaveChangesAsync();
        
        // Act
        var query = await repo.Query().ToListAsync();

        // Assert
        Assert.Equal(2, query.Count);
    }

}