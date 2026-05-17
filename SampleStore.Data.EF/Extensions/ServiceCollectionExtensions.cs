using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SampleStore.Data.EF.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddEntityFrameworkDataAccess(this IServiceCollection services, string connectionString)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddDbContext<DbContext, SampleStoreContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddSingleton<IQueryMaterializer, EfQueryMaterializer>();
    }
}