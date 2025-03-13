using GenericRepository;
using MediatR;
using OzdamarDepo.Domain.MediaItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace OzdamarDepo.Application.MediaItems;
public sealed record KargoDeleteCommand(
    Guid Id) : IRequest<Result<string>>;

internal sealed class KargoDeleteCommandHandler(IMediaItemRepository mediaItemRepository, IUnitOfWork unitOfWork) : IRequestHandler<KargoDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(KargoDeleteCommand request, CancellationToken cancellationToken)
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



