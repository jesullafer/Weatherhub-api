using WeatherHub.Application.Abstractions;

public class InMemoryFavoriteRepository : IFavoriteRepository
{
    private readonly List<string> _favorites = new();

    public void Add(string city)
    {
        _favorites.Add(city);
    }
}