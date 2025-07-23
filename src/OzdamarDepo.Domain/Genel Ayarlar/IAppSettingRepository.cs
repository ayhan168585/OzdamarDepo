using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzdamarDepo.Domain.Genel_Ayarlar
{
    public interface IAppSettingRepository
    {
        Task<List<AppSetting>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<AppSetting?> GetByKeyAsync(string key, CancellationToken cancellationToken = default);
        Task AddAsync(AppSetting setting, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);


        void Update(AppSetting setting);
    }
}
