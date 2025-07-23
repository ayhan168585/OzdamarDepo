using GenericRepository;
using MediatR;
using OzdamarDepo.Domain.Genel_Ayarlar;
using TS.Result;

namespace OzdamarDepo.Application.GenelAyarlar.Commands;

public sealed class AppSettingDeleteCommandHandler(
    IAppSettingRepository appSettingRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<AppSettingDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(AppSettingDeleteCommand request, CancellationToken cancellationToken)
    {
        var setting = await appSettingRepository.GetByKeyAsync(request.Key, cancellationToken);
        if (setting is null)
        {
            return Result<string>.Failure("Belirtilen ayar bulunamadı.");
        }

        setting.IsDeleted = true; // 🌟 Soft delete
        appSettingRepository.Update(setting);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed("Ayar silindi.");
    }
}
