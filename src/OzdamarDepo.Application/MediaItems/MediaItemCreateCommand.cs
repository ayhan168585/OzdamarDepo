using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using OzdamarDepo.Domain.MediaItems;
using TS.Result;

namespace OzdamarDepo.Application.MediaItems
{
    public sealed record MediaItemCreateCommand(
        string Title,
        string imageUrl,
        string ArtistOrActor,
        MediaType MediaType,
        decimal Price,
        DateOnly ReleaseDate,
        MediaCondition MediaCondition,
        bool IsBoxSet,
        int DiscCount) : IRequest<Result<string>>;

    public sealed class MediaItemCreateCommandValidator : AbstractValidator<MediaItemCreateCommand>
    {
        public MediaItemCreateCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Başlık boş olamaz!");
            RuleFor(x => x.ArtistOrActor).MinimumLength(2).WithMessage("En az 2 karakter olmalıdır!");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır!");
            RuleFor(x => x.MediaCondition.ConditionScore)
                .InclusiveBetween(1, 10).WithMessage("Durum puanı 1-10 arası olmalıdır!");
        }
    }

    public sealed class MediaItemCreateCommandHandler(
        IMediaItemRepository mediaItemRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<MediaItemCreateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(MediaItemCreateCommand request, CancellationToken cancellationToken)
        {

            MediaItem mediaItem = request.Adapt<MediaItem>();

            mediaItem.Title = request.Title;
            mediaItem.ImageUrl = request.imageUrl;
            mediaItem.ArtistOrActor = request.ArtistOrActor;
            mediaItem.Price = request.Price;
            mediaItem.MediaType= request.MediaType;
            mediaItem.ReleaseDate = request.ReleaseDate;
            mediaItem.MediaCondition = request.MediaCondition;
            mediaItem.MediaDurum=MediaDurumEnum.Bekliyor;
            mediaItem.IsBoxSet=request.IsBoxSet;
            mediaItem.DiscCount = request.DiscCount;
            await mediaItemRepository.AddAsync(mediaItem);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Medya öğesi başarıyla eklendi!");
        }

        private static string GetConditionDescription(int score) => score switch
        {
            >= 9 => "Mükemmel durumda",
            >= 7 => "İyi durumda",
            >= 5 => "Orta durumda",
            _ => "Kötü durumda"
        };
    }
} 