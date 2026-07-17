using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using WZCNet.Contexts;
using WZCNet.Converters;
using WZCNet.Exeptions;
using WZCNet.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<WZCNetDbContext>(DbContextOptions =>
{
    DbContextOptions.UseNpgsql(builder.Configuration["ConnectionStrings:Default"]);
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
});

builder.Services.AddScoped<IEmployeeService,EmployeeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthorization();

app.MapControllers();
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        (int statusCode, string message) = exception switch
        {
            ConflictExeption ex => (409, ex.Message),
            //NotFoundException ex => (404, ex.Message),
            _ => (500, "An unexpected error occurred.")
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(new
        {
            status = statusCode,
            title = message
        });
    });
});
app.Run();
