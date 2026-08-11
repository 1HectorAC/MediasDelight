
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
            new MediaItem { Id = 1, MediaTypeId = 1, UserId = "1", Name = "Stuff", Rating = 5, TimeStamp = new DateTime(2025, 5, 5) },
            new MediaItem { Id = 2, MediaTypeId = 1, UserId = "1", Name = "Stuff", Rating = 5, TimeStamp = new DateTime(2025, 5, 5) }
        );
        await context.SaveChangesAsync();

        // Act
        var query = await repo.Query().ToListAsync();

        // Assert
        Assert.Equal(2, query.Count);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingId_ReturnMatchingMediaItem()
    {
        // Arrange
        var repo = RepositoryHelper.Create<MediaItem>(out var context);

        await context.MediaItems.AddRangeAsync(
            new MediaItem { Id = 1, MediaTypeId = 1, UserId = "1", Name = "Stuff", Rating = 5, TimeStamp = new DateTime(2025, 5, 5) },
            new MediaItem { Id = 2, MediaTypeId = 1, UserId = "1", Name = "Stuff2", Rating = 7, TimeStamp = new DateTime(2025, 5, 5) }
        );

        await context.SaveChangesAsync();

        // Act
        var id = 1;
        var result = await repo.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task AddAsync_WithNoMediaItems_AddMediaItemtoDatabase()
    {
        // Arrange
        var repo = RepositoryHelper.Create<MediaItem>(out var context);
        var mediaItem = new MediaItem { Id = 2, MediaTypeId = 1, UserId = "1", Name = "Stuff2", Rating = 7, TimeStamp = new DateTime(2025, 5, 5) };
        // Act
        await repo.AddAsync(mediaItem);
        await repo.SaveChangesAsync();

        // Assert
        Assert.Equal(1, context.MediaItems.Count());
    }

    [Fact]
    public async Task Remove_WithMediaItems_RemoveMediaItemFromDatabase()
    {
        // Arrange
        var repo = RepositoryHelper.Create<MediaItem>(out var context);
        var mediaItem = new MediaItem { Id = 2, MediaTypeId = 1, UserId = "1", Name = "Stuff2", Rating = 7, TimeStamp = new DateTime(2025, 5, 5) };
        await repo.AddAsync(mediaItem);
        await repo.SaveChangesAsync();

        await context.SaveChangesAsync();

        // Act
        repo.Remove(mediaItem);
        await repo.SaveChangesAsync();

        // Assert
        Assert.Empty(context.MediaItems);
    }

    [Fact]
    public async Task SaveChangesAsync_WithPendingChanges_PersistDataToDatabase()
    {
        // Arrange
        var repo = RepositoryHelper.Create<MediaItem>(out var context);
        var mediaItem = new MediaItem { Id = 2, MediaTypeId = 1, UserId = "1", Name = "Stuff2", Rating = 7, TimeStamp = new DateTime(2025, 5, 5) };
        
        // Act
        await repo.AddAsync(mediaItem);
        await repo.SaveChangesAsync();

        // Assert
        Assert.NotEmpty(context.MediaItems);
    }

}