using ElasticSearchExample.MVC.Extensions;
using ElasticSearchExample.MVC.Mapping;
using ElasticSearchExample.MVC.Repositories;
using ElasticSearchExample.MVC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Elastic Search Singleton olarak servislerin arasýna ekliyoruz.
builder.Services.AddElasticSearch(builder.Configuration);

builder.Services.AddScoped<BlogRepository>();
builder.Services.AddScoped<BlogService>();

// Automapper ekliyoruz.
builder.Services.AddAutoMapper(typeof(BlogMapping).Assembly);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Blog}/{action=Index}/{id?}");

app.Run();
