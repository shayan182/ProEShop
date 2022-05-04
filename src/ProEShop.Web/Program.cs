using ProEShop.IocConfig;
using ProEShop.ViewModels.Identity.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//bind your value from appsetting to SiteSettings
builder.Services.Configure<SiteSettings>(options => builder.Configuration.Bind(options));
builder.Services.AddControllersWithViews();
builder.Services.AddCustomIdentityServices();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
