using MediatR;

namespace WeatherHub.Application.UseCases.Favorites.GetFavorites
{
    public class GetFavoritesQuery : IRequest<List<string>>
    {
    }
}
