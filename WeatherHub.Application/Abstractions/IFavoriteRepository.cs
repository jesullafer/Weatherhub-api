namespace WeatherHub.Application.Abstractions
{
    public interface IFavoriteRepository
    {
        void Add(string city);
        List<string> GetAll();
    }
}