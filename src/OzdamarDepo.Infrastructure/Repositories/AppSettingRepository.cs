using Microsoft.EntityFrameworkCore;
using OzdamarDepo.Domain.Genel_Ayarlar;
using OzdamarDepo.Infrastructure.Context;

namespace OzdamarDepo.Infrastructure.Repositories
{
    public sealed class AppSettingRepository : IAppSettingRepository
    {
        private readonly ApplicationDbContext _context;

        public AppSettingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AppSetting setting, CancellationToken cancellationToken = default)
        {
            await _context.AppSettings.AddAsync(setting, cancellationToken);
        }

        public async Task<List<AppSetting>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.AppSettings.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<AppSetting?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
        {
            return await _context.AppSettings.FirstOrDefaultAsync(x => x.Key == key, cancellationToken);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Update(AppSetting setting)
        {
            _context.AppSettings.Update(setting);
        }
    }

}
