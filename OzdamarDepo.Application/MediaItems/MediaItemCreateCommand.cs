using FluentValidation;
using GenericRepository;
using MediatR;
using OzdamarDepo.Domain.MediaItems;
using TS.Result;

namespace OzdamarDepo.Application.MediaItems
{
    public sealed record MediaItemCreateCommand(
        string Title,
        string ArtistOrDirector,
        string MediaFormat,
        string Category,
        decimal Price,
        DateOnly ReleaseDate,
        int ConditionScore) : IRequest<Result<string>>;

    public sealed class MediaItemCreateCommandValidator : AbstractValidator<MediaItemCreateCommand>
    {
        public MediaItemCreateCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Başlık boş olamaz!");
            RuleFor(x => x.ArtistOrDirector).MinimumLength(3).WithMessage("En az 3 karakter olmalıdır!");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır!");
            RuleFor(x => x.ConditionScore)
                .InclusiveBetween(1, 10).WithMessage("Durum puanı 1-10 arası olmalıdır!");
        }
    }

    internal sealed class MediaItemCreateCommandHandler(
        IMediaItemRepository mediaItemRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<MediaItemCreateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(MediaItemCreateCommand request, CancellationToken cancellationToken)
        {
            var mediaType = new MediaType 
            {
                Format = request.MediaFormat,
                Category = request.Category
            };

            var condition = new MediaCondition
            {
                ConditionScore = request.ConditionScore,
                Description = GetConditionDescription(request.ConditionScore)
            };

            MediaItem mediaItem = new()
            {
                Title = request.Title,
                ArtistOrDirector = request.ArtistOrDirector,
                MediaType = mediaType,
                Price = request.Price,
                ReleaseDate = request.ReleaseDate,
                Condition = condition
            };

            await mediaItemRepository.AddAsync(mediaItem, cancellationToken);
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