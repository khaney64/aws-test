using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;
using System.Configuration;
using WebApp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddSingleton<IAppConfiguration, AppConfiguration>();
builder.Services.AddSingleton<ILocationProvider>(GetLocationProvider);

ILocationProvider GetLocationProvider(IServiceProvider sp)
{
	var factory = sp.GetRequiredService<IHttpClientFactory>();
	var config = sp.GetRequiredService<IAppConfiguration>();
	if (config.Provider == ProviderType.MemCache)
		return new MemCacheLocationProvider();
	else
		return new WebApiLocationProvider(factory, config);
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
