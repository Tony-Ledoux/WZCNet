
using Microsoft.EntityFrameworkCore;
using WZCNet.src.Application.Converters;
using WZCNet.src.Application.Services;
using WZCNet.src.Infrastructure.Persistence.Contexts;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<WZCNetDbContext>(DbContextOptions =>
{
    DbContextOptions.UseNpgsql(builder.Configuration["ConnectionStrings:Default"]);
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddScoped<IEmployeeService,EmployeeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthorization();

app.MapControllers();

app.Run();
