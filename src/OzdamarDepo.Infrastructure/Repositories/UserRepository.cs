using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;
using OzdamarDepo.Domain.Users;
using OzdamarDepo.Infrastructure.Context;

namespace OzdamarDepo.Infrastructure.Repositories
{
    internal sealed class UserRepository : Repository<AppUser, ApplicationDbContext>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
