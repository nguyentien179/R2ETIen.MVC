using Microsoft.EntityFrameworkCore;
using R2ETien.MVC.Data;
using R2ETien.MVC.Interface;
using R2ETien.MVC.Middleware;
using R2ETien.MVC.Repositories;
using R2ETien.MVC.Service;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("MVC");

builder.Services.AddSqlite<Context>(connString);

builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

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

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.MigrateDb();

app.Run();
