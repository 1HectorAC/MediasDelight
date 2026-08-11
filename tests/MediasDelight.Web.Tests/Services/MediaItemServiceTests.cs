
using MediasDelight.Web.Models;
using MediasDelight.Web.Services.Implementations;
using MediasDelight.Web.Tests.Helpers;

namespace MediasDelight.Web.Tests.Services;

public class MediaItemServiceTests
{
    [Fact]
    public async Task GetAllByUserIdAsync_WithNoMediaItems_returnEmptyList()
    {
        // Arrange
        var repo = RepositoryHelper.Create<MediaItem>(out var context);
        var service = new MediaItemService(repo);

        // Act
        string userId = "1";
        var mediaItems = await service.GetAllByUserIdAsync(userId);

        // Assert
        Assert.Empty(mediaItems);
    }

    [Fact]
    public async Task GetAllByUserIdAsync_IncludingSingleMediaItemWithMatchingId_returnMediaItemWithMatchingUserId()
    {
        // Arrange
        var repo = RepositoryHelper.Create<MediaItem>(out var context);
        var service = new MediaItemService(repo);
        await context.MediaItems.AddRangeAsync(
            new MediaItem { Id = 1, MediaTypeId = 1, UserId = "1", Name = "Stuff", Rating = 5, TimeStamp = new DateTime(2025, 5, 5) },
            new MediaItem { Id = 2, MediaTypeId = 1, UserId = "2", Name = "Stuff", Rating = 5, TimeStamp = new DateTime(2025, 5, 5) }
        );

        await context.MediaTypes.AddRangeAsync(
            new MediaType {Id = 1, Name = "Tv"}
        );

        await context.SaveChangesAsync();

        // Act
        string userId = "1";
        var mediaItems = await service.GetAllByUserIdAsync(userId);
        var mediaItem = mediaItems.FirstOrDefault();

        // Assert
        Assert.NotNull(mediaItem);
        Assert.Equal(userId, mediaItem.UserId);
    }

    [Fact]
    public async Task GetAllByUserIdAsync_IncludingMultipleMediaItemWithMultipleMatchingId_returnMediaItemWithMatchingUserId()
    {
        // Arrange
        var repo = RepositoryHelper.Create<MediaItem>(out var context);
        var service = new MediaItemService(repo);

        await context.MediaItems.AddRangeAsync(
            new MediaItem { Id = 1, MediaTypeId = 1, UserId = "1", Name = "Stuff", Rating = 5, TimeStamp = new DateTime(2025, 5, 5) },
            new MediaItem { Id = 2, MediaTypeId = 1, UserId = "1", Name = "Stuff", Rating = 5, TimeStamp = new DateTime(2025, 5, 5) }
        );
        await context.SaveChangesAsync();

        // Act
        string userId = "1";
        var mediaItems = await service.GetAllByUserIdAsync(userId);

        // Assert
        Assert.NotNull(mediaItems);
        Assert.True(mediaItems.All(i => i.UserId == userId));
    }

    [Fact]
    public async Task GetAllByUserIdAndMediaTypeIdAsync_WithNoMediaItems_returnEmptyList()
    {
        // Arrange
        var repo = RepositoryHelper.Create<MediaItem>(out var context);
        var service = new MediaItemService(repo);

        // Act
        string userId = "1";
        int mediaTypeId = 1;
        var mediaItems = await service.GetAllByUserIdAndMediaTypeIdAsync(userId, mediaTypeId);

        // Assert
        Assert.Empty(mediaItems);
    }


    [Fact]
    public async Task GetAllByUserIdAndMediaTypeIdAsync_IncludingMultipleMediaItemWithMultipleMatchingIds_returnMediaItemWithMatchingUserId()
    {
        // Arrange
        var repo = RepositoryHelper.Create<MediaItem>(out var context);
        var service = new MediaItemService(repo);
        await context.MediaItems.AddRangeAsync(
            new MediaItem { Id = 1, MediaTypeId = 1, UserId = "1", Name = "Stuff", Rating = 5, TimeStamp = new DateTime(2025, 5, 5) },
            new MediaItem { Id = 2, MediaTypeId = 1, UserId = "1", Name = "Stuff", Rating = 5, TimeStamp = new DateTime(2025, 5, 5) }
        );
        await context.SaveChangesAsync();

        // Act
        string userId = "1";
        int mediaTypeId = 1;
        var mediaItems = await service.GetAllByUserIdAndMediaTypeIdAsync(userId, mediaTypeId);

        // Assert
        Assert.NotNull(mediaItems);
        Assert.True(mediaItems.All(i => i.UserId == userId && i.MediaTypeId == mediaTypeId));

    }

    [Fact]
    public async Task AddAsync_NoMediaItems_AddMediaItemToDatabase()
    {
        // Arrange
        var repo = RepositoryHelper.Create<MediaItem>(out var context);
        var service = new MediaItemService(repo);
        var mediaItem = new MediaItem { Id = 2, MediaTypeId = 1, UserId = "1", Name = "Stuff", Rating = 5, TimeStamp = new DateTime(2025, 5, 5) };

        // Act
        await service.AddAsync(mediaItem);

        // Assert
        Assert.NotEmpty(context.MediaItems);
        Assert.Equal(1, context.MediaItems.Count());
    }

    [Fact]
    public async Task DeleteAsync_NoMediaItems_ThrowException()
    {
        // Arrange
        var repo = RepositoryHelper.Create<MediaItem>(out var context);
        var service = new MediaItemService(repo);

        // Assert
        await Assert.ThrowsAsync<Exception>(async () =>
        {
            // Act
            await service.DeleteAsync(2);
        });
    }

    [Fact]
    public async Task DeleteAsync_WithMatchingMediaItemsId_DeleteMediaItemFromDatabase()
    {
        // Arrange
        var repo = RepositoryHelper.Create<MediaItem>(out var context);
        var service = new MediaItemService(repo);
        await context.MediaItems.AddRangeAsync(
            new MediaItem { Id = 1, MediaTypeId = 1, UserId = "1", Name = "Stuff", Rating = 5, TimeStamp = new DateTime(2025, 5, 5) },
            new MediaItem { Id = 2, MediaTypeId = 1, UserId = "1", Name = "Stuff", Rating = 5, TimeStamp = new DateTime(2025, 5, 5) }
        );
        await context.SaveChangesAsync();

        // Act
        await service.DeleteAsync(2);

        // Assert
        Assert.NotNull(context.MediaItems);
        Assert.Equal(1, context.MediaItems.Count());
    }

}