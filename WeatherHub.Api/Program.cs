using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using System.Text;
using WeatherHub.Api.Middleware;
using WeatherHub.Application.Abstractions;
using WeatherHub.Application.UseCases.GetWeatherByCity;
using WeatherHub.Infrastructure.Options;
using WeatherHub.Infrastructure.Services;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblyContaining<GetWeatherByCityValidator>();

builder.Services.AddScoped<GetWeatherByCityQueryHandler>();

builder.Services.Configure<AemetOptions>(
    builder.Configuration.GetSection("Aemet"));

builder.Services.AddHttpClient<AemetWeatherProvider>();
builder.Services.AddScoped<ICityCodeService, CityCodeService>();

builder.Services.AddMemoryCache();

builder.Services.AddScoped<IWeatherProvider>(sp =>
{
    var aemetProvider = sp.GetRequiredService<AemetWeatherProvider>();
    var cache = sp.GetRequiredService<IMemoryCache>();
    var logger = sp.GetRequiredService<ILogger<CachedWeatherProvider>>();

    return new CachedWeatherProvider(aemetProvider, cache, logger);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();