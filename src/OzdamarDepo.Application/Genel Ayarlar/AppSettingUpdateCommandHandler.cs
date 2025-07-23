using GenericRepository;
using MediatR;
using OzdamarDepo.Domain.Genel_Ayarlar;
using TS.Result;

namespace OzdamarDepo.Application.Genel_Ayarlar
{
    public sealed class AppSettingUpdateCommandHandler(
        IAppSettingRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<AppSettingUpdateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(AppSettingUpdateCommand request, CancellationToken cancellationToken)
        {
            var setting = await repository.GetByKeyAsync(request.Key, cancellationToken);
            if (setting is null)
                return Result<string>.Failure("Ayar bulunamadı!");

            setting.Value = request.Value;
            repository.Update(setting);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Ayar güncellendi.";
        }
    }

}
