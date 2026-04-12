using MediatR;

namespace WeatherHub.Application.UseCases.Favorites.AddFavorite;

public class AddFavoriteCommand : IRequest<Unit>
{
    public string City { get; }

    public AddFavoriteCommand(string city)
    {
        City = city?.Trim() ?? string.Empty;
    }
}