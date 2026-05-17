using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using SampleStore.Common.Extensions;

namespace SampleStore.Data.EF.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddEntityFrameworkDataAccess(this IServiceCollection services, string connectionString)
    {
        services.ThrowIfArgumentIsNull(nameof(services));

        services.AddDbContext<DbContext, SampleStoreContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddSingleton<IQueryMaterializer, EfQueryMaterializer>();
    }
}