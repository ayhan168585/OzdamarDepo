using GenericRepository;
using MediatR;
using OzdamarDepo.Domain.MediaItems;
using TS.Result;

namespace OzdamarDepo.Application.MediaItems;
public sealed record MediaItemDeleteCommand(
    Guid Id) : IRequest<Result<string>>;

public sealed class MediaItemDeleteCommandHandler(IMediaItemRepository mediaItemRepository, IUnitOfWork unitOfWork) : IRequestHandler<MediaItemDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(MediaItemDeleteCommand request, CancellationToken cancellationToken)
    {
        MediaItem? mediaItem = await mediaItemRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (mediaItem is null)
        {
            return Result<string>.Failure("Medya ürünü bulunamadı!");
        }

        if (mediaItem.MediaDurum != MediaDurumEnum.Bekliyor)
        {
            return Result<string>.Failure("Sadece bekleyen ürünleri silebilirsiniz!");
        }

        mediaItem.IsDeleted = true;
        mediaItemRepository.Update(mediaItem);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ("Kargo başarıyla silindi!");

    }
}



