using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProEShop.Common.GuardToolKit;
using ProEShop.Common.PersianToolKit;
using ProEShop.DataLayer.Context;
using ProEShop.ViewModels.Identity.Settings;

namespace ProEShop.IocConfig;

public static class DbContextOptionsExtensions
{
    public static IServiceCollection AddConfiguredDbContext
        (this IServiceCollection services, SiteSettings siteSettings)
    {
        siteSettings.CheckArgumentIsNull(nameof(siteSettings));
        var connectionString = siteSettings.ConnectionStrings.ApplicationDbContextConnection;
        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());
        // we use DbContextPool because it's faster than DbContext
        services.AddDbContextPool<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
            options.AddInterceptors(new PersianYeKeCommandInterceptor());
        });
        return services;
    }
}