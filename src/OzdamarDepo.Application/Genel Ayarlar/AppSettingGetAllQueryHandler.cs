using MediatR;
using OzdamarDepo.Domain.Genel_Ayarlar;

namespace OzdamarDepo.Application.Genel_Ayarlar
{
    public sealed class AppSettingGetAllQueryHandler(
        IAppSettingRepository repository) : IRequestHandler<AppSettingGetAllQuery, List<AppSetting>>
    {
        public async Task<List<AppSetting>> Handle(AppSettingGetAllQuery request, CancellationToken cancellationToken)
            => await repository.GetAllAsync(cancellationToken);
    }
}
