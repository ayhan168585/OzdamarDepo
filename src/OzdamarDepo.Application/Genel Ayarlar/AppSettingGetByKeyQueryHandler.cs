using MediatR;
using OzdamarDepo.Domain.Genel_Ayarlar;
using TS.Result;

namespace OzdamarDepo.Application.GenelAyarlar.Queries;

public sealed class AppSettingGetByKeyQueryHandler(
    IAppSettingRepository repository
) : IRequestHandler<AppSettingGetByKeyQuery, Result<string>>
{
    public async Task<Result<string>> Handle(AppSettingGetByKeyQuery request, CancellationToken cancellationToken)
    {
        var setting = await repository.GetByKeyAsync(request.Key, cancellationToken);

        return setting is null
            ? Result<string>.Failure("Belirtilen anahtar ile ayar bulunamadı.")
            : Result<string>.Succeed(setting.Value);
    }
}
