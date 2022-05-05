using DNTCommon.Web.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProEShop.Common.GuardToolKit;
using ProEShop.Common.PersianToolKit;
using ProEShop.DataLayer.Context;
using ProEShop.Services.Contracts.Identity;
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

    /// <summary>
    /// Creates and seeds the database.
    /// </summary>
    public static void InitializeDb(this IServiceProvider serviceProvider)
    {
        // we need to injection IIdentityDbInitializer,
        // we have to do the injection manually because this method run before Ioc

        // main code without using DNTCommon.Web.Core package

        //using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
        //{
        //    var context = serviceScope.ServiceProvider.GetRequiredService<IIdentityDbInitializer>();
        //    context.Initialize();
        //    context.SeedData();
        //}

        // use DNTCommon.Web.Core package for seed data

        serviceProvider.RunScopedService<IIdentityDbInitializer>(identityDbInitialize =>
        {
            identityDbInitialize.Initialize();
            identityDbInitialize.SeedData();
        });
    }
}