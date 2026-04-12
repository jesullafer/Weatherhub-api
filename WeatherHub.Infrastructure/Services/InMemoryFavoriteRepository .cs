using WeatherHub.Application.Abstractions;

namespace WeatherHub.Infrastructure.Services;

public class InMemoryFavoriteRepository : IFavoriteRepository
{
    private readonly List<string> _favorites = new();

    public void Add(string city)
    {
        _favorites.Add(city);
    }

    public List<string> GetAll()
    {
        return _favorites;
    }
}