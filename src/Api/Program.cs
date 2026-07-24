
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using WZCNet.src.Application.Converters;
using WZCNet.src.Infrastructure.Persistence.Contexts;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WZCNet.src.Application.Extensions;
using WZCNet.src.Api.Extensions;
using WZCNet.src.Infrastructure.http;
using WZCNet.src.Application.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddApplicationAuthentication(builder.Configuration);

builder.Services.AddDbContext<WZCNetDbContext>(DbContextOptions =>
{
    DbContextOptions.UseNpgsql(builder.Configuration["ConnectionStrings:Default"]);
});
builder.Services.AddScoped<RequestContext>();
builder.Services.AddScoped<IRequestContext>(sp=>sp.GetRequiredService<RequestContext>());

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();
app.UseMiddleware<RequestContextMiddleware>();
// Configure the HTTP request pipeline.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
