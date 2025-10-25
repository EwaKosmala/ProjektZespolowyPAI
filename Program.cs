using lab1_gr1.Interfaces;
using lab1_gr1.Models;
using lab1_gr1.Services;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using lab1_gr1.Configuration.AMProfile;
using ListaZakupow.Model.DataModels;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Rejestracja AutoMappera

// Rejestracja serwisów
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<PasswordHasher<User>>();

// builder.Services.AddScoped<IRecipeService, RecipeService>();

// MVC
builder.Services.AddControllersWithViews();

// DbContext
builder.Services.AddDbContext<MyDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProjektPAI")));

// Sesja
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MainProfile>());


var app = builder.Build();

// Sprawdzenie po³¹czenia z DB
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MyDBContext>();
    if (!dbContext.Database.CanConnect())
        throw new NotImplementedException("Can not connect to DB");
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

// Routing domyœlny
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Index}/{id?}");

app.Run();
