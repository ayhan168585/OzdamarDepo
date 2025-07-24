using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using OzdamarDepo.Domain.MediaItems;
using TS.Result;

namespace OzdamarDepo.Application.MediaItems;
public sealed record MediaItemUpdateCommand(
       Guid Id,
    string Title,
    string ImageUrl,           // ← resim URL'si
    string ArtistOrActor,
    MediaType MediaType,
    decimal Price,
    DateOnly ReleaseDate,
    MediaCondition MediaCondition,
    bool IsBoxSet,
    int DiscCount,
    byte[]? Image,               // ← yeni resim verisi varsa
    string? ImageFileName,      // ← dosya adı
    HttpRequest HttpRequest     // ← IFormFile erişimi için gerekli
) : IRequest<Result<string>>;

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
            return Result<string>.Failure("Medya bulunamadı!");

        if (mediaitem.MediaDurum != MediaDurumEnum.Bekliyor)
            return Result<string>.Failure("Sadece bekleyen medya ürünleri güncellenebilir!");

        // Map edilen alanlar
        request.Adapt(mediaitem);

        mediaitem.Title = request.Title;
        mediaitem.ArtistOrActor = request.ArtistOrActor;
        mediaitem.Price = request.Price;
        mediaitem.MediaType = request.MediaType;
        mediaitem.ReleaseDate = request.ReleaseDate;
        mediaitem.MediaCondition = request.MediaCondition;
        mediaitem.MediaDurum = MediaDurumEnum.Bekliyor;
        mediaitem.IsBoxSet = request.IsBoxSet;
        mediaitem.DiscCount = request.DiscCount;

        // ✅ YENİ RESİM VARSA GÜNCELLE ve SUNUCUYA YAZ
        if (request.Image is not null && !string.IsNullOrWhiteSpace(request.ImageFileName))
        {
            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsPath);

            // ✅ 2. Benzersiz dosya adı oluştur

            var cleanedFileName = request.ImageFileName.Replace(" ", "_");
            var uniqueFileName = $"{Guid.NewGuid()}_{cleanedFileName}";

            // 3. Dosya yolunu oluştur
            var filePath = Path.Combine(uploadsPath, uniqueFileName);

            // 4. Dosyayı yaz
            await File.WriteAllBytesAsync(filePath, request.Image!, cancellationToken);

            // ✅ Eski resmi sil (varsa)
            var oldFileName = Path.GetFileName(mediaitem.ImageUrl);
            var oldImagePath = Path.Combine("wwwroot", "uploads", oldFileName);
            if (File.Exists(oldImagePath))
            {
                File.Delete(oldImagePath);
            }

            // 4. Veritabanı alanlarını güncelle
            mediaitem.Image = request.Image;
            mediaitem.ImageUrl = $"{request.HttpRequest.Scheme}://{request.HttpRequest.Host}/uploads/{uniqueFileName}";

        }

        mediaItemRepository.Update(mediaitem);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Medya öğesi başarıyla güncellendi!";
    }
}


