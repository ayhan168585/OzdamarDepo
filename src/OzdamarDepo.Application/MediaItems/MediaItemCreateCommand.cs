using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using OzdamarDepo.Domain.MediaItems;
using TS.Result;

namespace OzdamarDepo.Application.MediaItems
{
    public sealed record MediaItemCreateCommand(
        string Title,
    string ArtistOrActor,
    MediaType MediaType,
    decimal Price,
    DateOnly ReleaseDate,
    MediaCondition MediaCondition,
    bool IsBoxSet,
    int DiscCount,
    byte[] Image,
    string ImageFileName,

    // 🔽 Bunu ekle
    HttpRequest HttpRequest
) : IRequest<Result<string>>;

    public sealed class MediaItemCreateCommandValidator : AbstractValidator<MediaItemCreateCommand>
    {
        public MediaItemCreateCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Başlık boş olamaz!");
            RuleFor(x => x.ArtistOrActor).MinimumLength(2).WithMessage("En az 2 karakter olmalıdır!");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır!");
            RuleFor(x => x.MediaCondition.ConditionScore)
                .InclusiveBetween(1, 10).WithMessage("Durum puanı 1-10 arası olmalıdır!");
            RuleFor(x => x.Image).NotEmpty().WithMessage("Resim dosyası zorunludur!");
            RuleFor(x => x.ImageFileName).NotEmpty().WithMessage("Resim dosya adı zorunludur!");
        }
    }

    public sealed class MediaItemCreateCommandHandler(
        IMediaItemRepository mediaItemRepository,
        IUnitOfWork unitOfWork,
        IWebHostEnvironment env
    ) : IRequestHandler<MediaItemCreateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(MediaItemCreateCommand request, CancellationToken cancellationToken)
        {
            // WebRootPath boşsa fallback olarak çalış
            var rootPath = env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadsPath = Path.Combine(rootPath, "uploads");

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.ImageFileName)}";
            var imagePath = Path.Combine(uploadsPath, fileName);
            var httpRequest = request.HttpRequest;

            Directory.CreateDirectory(uploadsPath); // Klasör varsa bir şey yapmaz


            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(request.ImageFileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                return Result<string>.Failure("Sadece JPG, PNG veya WEBP uzantılı dosyalar destekleniyor.");
            }

            await File.WriteAllBytesAsync(imagePath, request.Image, cancellationToken);

            var mediaItem = request.Adapt<MediaItem>();
            mediaItem.ImageUrl = $"{httpRequest.Scheme}://{httpRequest.Host}/uploads/{fileName}";
            mediaItem.MediaDurum = MediaDurumEnum.Bekliyor;
            var cleanedFileName = request.ImageFileName.Replace(" ", "_");
            var uniqueFileName = $"{Guid.NewGuid()}_{cleanedFileName}";

            await mediaItemRepository.AddAsync(mediaItem);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Medya öğesi başarıyla eklendi!");
        }

    }
}
