using MediatR;
using OzdamarDepo.Domain.MediaItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace OzdamarDepo.Application.MediaItems
{
    public sealed record MediaItemGetQuery(
        Guid Id) : IRequest<Result<MediaItem>>;

    internal sealed class MediaItemGetQueryHandler(IMediaItemRepository mediaItemRepository) : IRequestHandler<MediaItemGetQuery, Result<MediaItem>>
    {
        public async Task<Result<MediaItem>> Handle(MediaItemGetQuery request, CancellationToken cancellationToken)
        {
            var employee = await mediaItemRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (employee is null)
            {
                return Result<MediaItem>.Failure("Personel bulunamadı!");
            }

            return employee;
        }
    }
}
