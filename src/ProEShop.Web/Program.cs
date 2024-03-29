using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using AutoMapper;
using DNTCommon.Web.Core;
using Microsoft.Extensions.WebEncoders;
using ProEShop.IocConfig;
using ProEShop.ViewModels.Identity.Settings;
using ProEShop.Web.Mappings;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

//bind your value from appsetting to SiteSettings
builder.Services.Configure<SiteSettings>(options => builder.Configuration.Bind(options));
builder.Services.Configure<ContentSecurityPolicyConfig>(options => builder.Configuration.GetSection("ContentSecurityPolicyConfig").Bind(options));
builder.Services.AddCustomIdentityServices();
builder.Services.AddRazorPages();
builder.Services.Configure<WebEncoderOptions>(options =>
{
    options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
});

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddMaps(Assembly.GetExecutingAssembly());
});
var mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();
app.Services.InitializeDb();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseContentSecurityPolicy();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.Run();
