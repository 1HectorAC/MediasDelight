
using MediasDelight.Web.Models;
using MediasDelight.Web.Services.Implementations;
using MediasDelight.Web.Tests.Helpers;

namespace MediasDelight.Web.Tests.Services;

public class MediaTypeServiceTests
{

    [Fact]
    public async Task GetAllAsync_WithNoMediaTypes_ReturnEmptyList()
    {
        // Arrange
        var repo = RepositoryHelper.Create<MediaType>(out var context);
        var service = new MediaTypeService(repo);

        // Act
        var mediaTypes = await service.GetAllAsync();

        // Assert
        Assert.Empty(mediaTypes);
    }

    [Fact]
    public async Task GetAllAsync_WithMediaTypes_ReturnList()
    {
        // Arrange
        var repo = RepositoryHelper.Create<MediaType>(out var context);
        var service = new MediaTypeService(repo);

        await context.MediaTypes.AddRangeAsync(
            new MediaType { Id = 1, Name = "Tv Show" },
            new MediaType { Id = 2, Name = "Movie" }

        );
        await context.SaveChangesAsync();

        // Act
        var mediaTypes = await service.GetAllAsync();

        // Assert
        Assert.NotEmpty(mediaTypes);
    }

    [Fact]
    public async Task GetByIdAsync_WithNoMediaTypes_ReturnNull()
    {
        // Arrange
        var repo = RepositoryHelper.Create<MediaType>(out var context);
        var service = new MediaTypeService(repo);

        // Act
        var id = 1;
        var mediaType = await service.GetByIdAsync(id);

        // Assert
        Assert.Null(mediaType);
    }

    [Fact]
    public async Task GetByIdAsync_WithMediaTypes_ReturnMediaTypeWithMatchingId()
    {
        // Arrange
        var repo = RepositoryHelper.Create<MediaType>(out var context);
        var service = new MediaTypeService(repo);

        await context.MediaTypes.AddRangeAsync(
            new MediaType { Id = 1, Name = "Tv Show" },
            new MediaType { Id = 2, Name = "Movie" }

        );
        await context.SaveChangesAsync();

        // Act
        var id = 1;
        var mediaType = await service.GetByIdAsync(id);

        // Assert
        Assert.NotNull(mediaType);
        Assert.Equal(id, mediaType.Id);
    }

}