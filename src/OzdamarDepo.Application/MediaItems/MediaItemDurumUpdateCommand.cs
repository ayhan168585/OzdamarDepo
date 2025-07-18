using FluentValidation;
using GenericRepository;
using MediatR;
using OzdamarDepo.Domain.MediaItems;
using TS.Result;

namespace OzdamarDepo.Application.MediaItems;
public sealed record MediaItemDurumUpdateCommand(
    Guid Id,
    int DurumValue) : IRequest<Result<string>>;


public sealed class MediaItemDurumUpdateCommandValidator : AbstractValidator<MediaItemDurumUpdateCommand>
{
    public MediaItemDurumUpdateCommandValidator()
    {
        RuleFor(p => p.DurumValue).GreaterThanOrEqualTo(0).WithMessage("Geçerli bir durum girin!").LessThanOrEqualTo(1).WithMessage("Geçerli bir durum girin!");
    }
}

public sealed class MediaItemDurumUpdateCommandHandler(IMediaItemRepository mediaItemRepository, IUnitOfWork unitOfWork) : IRequestHandler<MediaItemDurumUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(MediaItemDurumUpdateCommand request, CancellationToken cancellationToken)
    {
        MediaItem? mediaItem = await mediaItemRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (mediaItem is null)
        {
            return Result<string>.Failure("Medya bulunamadı!");
        }

        mediaItem.MediaDurum = (MediaDurumEnum)request.DurumValue;

        mediaItemRepository.Update(mediaItem);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Medya durumu Başarı ile güncellendi!";

    }
}


