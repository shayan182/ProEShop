using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProEShop.DataLayer.Context;

namespace ProEShop.IocConfig;
public static class RegisterServices
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer("Data Source=.;Initial Catalog=ProEShop;Trusted_Connection=True;MultipleActiveResultSets=True");
        });

        return services;
    }
}
