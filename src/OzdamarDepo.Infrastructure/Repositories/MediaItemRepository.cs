using GenericRepository;
using OzdamarDepo.Domain.MediaItems;
using OzdamarDepo.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzdamarDepo.Infrastructure.Repositories
{
    internal sealed class MediaItemRepository: Repository<MediaItem, 
    ApplicationDbContext>, IMediaItemRepository
    {
        public MediaItemRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
