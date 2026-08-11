
using MediasDelight.Web.Data;
using MediasDelight.Web.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace MediasDelight.Web.Tests.Helpers;

public static class RepositoryHelper
{
    public static GenericRepository<T> Create<T>(out AppDbContext context)
    where T : class
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new AppDbContext(options);
        return new GenericRepository<T>(context);
    }
}