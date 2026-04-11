using MediatR;
using WeatherHub.Application.Abstractions;

namespace WeatherHub.Application.UseCases.Favorites.AddFavorite;

public class AddFavoriteCommandHandler : IRequestHandler<AddFavoriteCommand, Unit>
{
    private readonly IFavoriteRepository _repository;

    public AddFavoriteCommandHandler(IFavoriteRepository repository)
    {
        _repository = repository;
    }

    public Task<Unit> Handle(AddFavoriteCommand request, CancellationToken cancellationToken)
    {
        _repository.Add(request.City);

        return Task.FromResult(Unit.Value);
    }
}