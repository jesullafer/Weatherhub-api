using MediatR;
using WeatherHub.Application.Abstractions;

namespace WeatherHub.Application.UseCases.Favorites.GetFavorites
{
    public class GetFavoritesQueryHandler : IRequestHandler<GetFavoritesQuery, List<string>>
    {
        private readonly IFavoriteRepository _repository;

        public GetFavoritesQueryHandler(IFavoriteRepository repository)
        {
            _repository = repository;
        }

        public Task<List<string>> Handle(GetFavoritesQuery request, CancellationToken cancellationToken)
        {
            var favorites = _repository.GetAll();

            return Task.FromResult(favorites);
        }
    }
}
