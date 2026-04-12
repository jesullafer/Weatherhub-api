using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Serilog;
using System.Text;
using WeatherHub.Api.Middleware;
using WeatherHub.Application.Abstractions;
using WeatherHub.Application.Common.Behaviors;
using WeatherHub.Application.UseCases.GetWeatherByCity;
using WeatherHub.Infrastructure.Options;
using WeatherHub.Infrastructure.Services;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
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

builder.Services.Configure<CacheOptions>(
    builder.Configuration.GetSection("Cache"));

builder.Services.AddHttpClient<AemetWeatherProvider>();
builder.Services.AddScoped<ICityCodeService, CityCodeService>();
builder.Services.AddScoped<ICitySearchService, CitySearchService>();
builder.Services.AddSingleton<IFavoriteRepository, InMemoryFavoriteRepository>();

builder.Services.AddMemoryCache();

builder.Services.AddScoped<IWeatherProvider>(sp =>
{
    var aemetProvider = sp.GetRequiredService<AemetWeatherProvider>();
    var cache = sp.GetRequiredService<IMemoryCache>();
    var logger = sp.GetRequiredService<ILogger<CachedWeatherProvider>>();
    var options = sp.GetRequiredService<IOptions<CacheOptions>>();

    return new CachedWeatherProvider(aemetProvider, cache, logger, options);
});

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GetWeatherByCityQueryHandler).Assembly));

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

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