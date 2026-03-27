namespace WeatherHub.Application.Abstractions
{
    public interface ICityCodeService
    {
        Task<string?> GetCode(string city);
    }
}
