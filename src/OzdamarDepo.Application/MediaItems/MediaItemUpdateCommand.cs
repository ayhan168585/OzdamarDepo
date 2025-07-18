using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using OzdamarDepo.Domain.MediaItems;
using TS.Result;

namespace OzdamarDepo.Application.MediaItems;
public sealed record MediaItemUpdateCommand(
       Guid Id,
       string Title,
       string ImageUrl,
       string ArtistOrActor,
       MediaType MediaType,
       decimal Price,
       DateOnly ReleaseDate,
       MediaCondition MediaCondition,
       bool IsBoxSet,
       int DiscCount) : IRequest<Result<string>>;

public sealed class MediaItemUpdateCommandValidator : AbstractValidator<MediaItemUpdateCommand>
{
    public MediaItemUpdateCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Başlık boş olamaz!");
        RuleFor(x => x.ArtistOrActor).MinimumLength(2).WithMessage("En az 2 karakter olmalıdır!");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır!");
        RuleFor(x => x.MediaCondition.ConditionScore)
            .InclusiveBetween(1, 10).WithMessage("Durum puanı 1-10 arası olmalıdır!");
    }
}

public sealed class MediaItemUpdateCommandHandler(
        IMediaItemRepository mediaItemRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<MediaItemUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(MediaItemUpdateCommand request, CancellationToken cancellationToken)
    {

        var mediaitem = await mediaItemRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (mediaitem is null)
        {
            return Result<string>.Failure("Medya bulunamadı!");
        }

        if (mediaitem.MediaDurum != MediaDurumEnum.Bekliyor)
        {
            return Result<string>.Failure("Sadece bekleyen medya ürünleri geüncellenebilir!");
        }

        request.Adapt(mediaitem);

        mediaitem.Title = request.Title;
        mediaitem.ImageUrl = request.ImageUrl;
        mediaitem.ArtistOrActor = request.ArtistOrActor;
        mediaitem.Price = request.Price;
        mediaitem.MediaType = request.MediaType;
        mediaitem.ReleaseDate = request.ReleaseDate;
        mediaitem.MediaCondition = request.MediaCondition;
        mediaitem.MediaDurum = MediaDurumEnum.Bekliyor;
        mediaitem.IsBoxSet = request.IsBoxSet;
        mediaitem.DiscCount = request.DiscCount;


        mediaItemRepository.Update(mediaitem);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Medya öğesi başarıyla güncellendi!";
    }

    private static string GetConditionDescription(int score) => score switch
    {
        >= 9 => "Mükemmel durumda",
        >= 7 => "İyi durumda",
        >= 5 => "Orta durumda",
        _ => "Kötü durumda"
    };
}

