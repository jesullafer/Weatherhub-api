using System.Text;
using WeatherHub.Application.Abstractions;
using WeatherHub.Application.UseCases.GetWeatherByCity;
using WeatherHub.Infrastructure.Options;
using WeatherHub.Infrastructure.Services;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<GetWeatherByCityQueryHandler>();

builder.Services.Configure<AemetOptions>(
    builder.Configuration.GetSection("Aemet"));

builder.Services.AddHttpClient<AemetWeatherProvider>();

builder.Services.AddScoped<IWeatherProvider, AemetWeatherProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
