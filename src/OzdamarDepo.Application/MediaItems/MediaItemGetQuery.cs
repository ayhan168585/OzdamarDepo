using MediatR;
using OzdamarDepo.Domain.MediaItems;
using TS.Result;

namespace OzdamarDepo.Application.MediaItems
{
    public sealed record MediaItemGetQuery(
        Guid Id) : IRequest<Result<MediaItem>>;

    public sealed class MediaItemGetQueryHandler(IMediaItemRepository mediaItemRepository) : IRequestHandler<MediaItemGetQuery, Result<MediaItem>>
    {
        public async Task<Result<MediaItem>> Handle(MediaItemGetQuery request, CancellationToken cancellationToken)
        {
            var mediaItem = await mediaItemRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (mediaItem is null)
            {
                return Result<MediaItem>.Failure("Medya ürünü bulunamadı!");
            }

            return mediaItem;
        }
    }
}
