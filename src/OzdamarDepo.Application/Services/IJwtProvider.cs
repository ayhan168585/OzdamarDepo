using OzdamarDepo.Domain.Users;

namespace OzdamarDepo.Application.Services
{
    public interface IJwtProvider
    {
        public Task<string> CreateTokenAsync(AppUser user, CancellationToken cancellationToken = default);
    }
}
