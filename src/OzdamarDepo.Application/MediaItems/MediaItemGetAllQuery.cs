using OzdamarDepo.Domain.Abstractions;
using OzdamarDepo.Domain.MediaItems;
using MediatR;

namespace OzdamarDepo.Application.MediaItems;

public sealed record MediaItemGetAllQuery() : IRequest<IQueryable<MediaItemGetAllQueryResponse>>;

public sealed class MediaItemGetAllQueryResponse : EntityDto
{
    public string Title { get; set; } = default!;
    public string ArtistOrDirector { get; set; } = default!;
    public string MediaFormat { get; set; } = default!;
    public string Category { get; set; } = default!;
    public decimal Price { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public int ConditionScore { get; set; }
}

internal sealed class MediaItemGetAllQueryHandler(IMediaItemRepository mediaItemRepository)
    : IRequestHandler<MediaItemGetAllQuery, IQueryable<MediaItemGetAllQueryResponse>>
{
    public Task<IQueryable<MediaItemGetAllQueryResponse>> Handle(
        MediaItemGetAllQuery request, 
        CancellationToken cancellationToken)
    {
        var response = mediaItemRepository
            .GetAll()
            .Select(x => new MediaItemGetAllQueryResponse
            {
                Id = x.Id,
                Title = x.Title,
                ArtistOrDirector = x.ArtistOrDirector,
                MediaFormat = x.MediaType.Format,
                Category = x.MediaType.Category,
                Price = x.Price,
                ReleaseDate = x.ReleaseDate,
                ConditionScore = x.MediaCondition.ConditionScore,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                IsDeleted = x.IsDeleted,
                DeletedAt = x.DeletedAt
            });
            
        return Task.FromResult(response);
    }
} 