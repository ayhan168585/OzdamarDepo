using GenericRepository;
using MediatR;
using OzdamarDepo.Domain.Genel_Ayarlar;
using TS.Result;

namespace OzdamarDepo.Application.GenelAyarlar.Commands;

public sealed class AppSettingCreateCommandHandler(
    IAppSettingRepository appSettingRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<AppSettingCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(AppSettingCreateCommand request, CancellationToken cancellationToken)
    {
        var existing = await appSettingRepository.GetByKeyAsync(request.Key, cancellationToken);

        if (existing is not null)
            return Result<string>.Failure("❌ Bu anahtara sahip bir ayar zaten mevcut.");

        var setting = new AppSetting
        {
            Key = request.Key,
            Value = request.Value,
            ValueType = request.ValueType
        };

        await appSettingRepository.AddAsync(setting, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("✅ Ayar eklendi.");
    }
}


