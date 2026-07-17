using Microsoft.EntityFrameworkCore;
using WZCNet.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<WZCNetDbContext>(DbContextOptions =>
{
    DbContextOptions.UseNpgsql(builder.Configuration["ConnectionStrings:Default"]);
});

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
