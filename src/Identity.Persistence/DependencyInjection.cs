using Identity.Persistence.DataContexts;
using Identity.Persistence.Interceptors;
using Identity.Persistence.Repositories;
using Identity.Persistence.UnitOfWorks;
using Identity.Persistence.UnitOfWorks.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // Interceptors
        services.AddScoped<AuditableInterceptor>();
        services.AddScoped<SoftDeletedInterceptor>();

        services.AddDbContext<DbContext, AppDbContext>((provider, options) =>
        {
            var auditableInterceptor = provider.GetRequiredService<AuditableInterceptor>();
            var softDeletedInterceptor = provider.GetRequiredService<SoftDeletedInterceptor>();

            options
                .UseNpgsql(configuration.GetConnectionString("DefaultDbConnection"))
                .AddInterceptors(auditableInterceptor)
                .AddInterceptors(softDeletedInterceptor);
        });

        return services;
    }
}