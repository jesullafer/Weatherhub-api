using MediatR;

namespace WeatherHub.Application.UseCases.Favorites.AddFavorite;

public record AddFavoriteCommand(string City) : IRequest<Unit>;