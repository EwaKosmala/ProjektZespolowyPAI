using lab1_gr1.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MyDBContext>(options => { options.UseSqlServer(builder.Configuration.GetConnectionString("ProjektPAI")); });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MyDBContext>();

    if (!dbContext.Database.CanConnect())
    {
        throw new NotImplementedException("Can not connect to DB");
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapDefaultControllerRoute();

app.Run();
