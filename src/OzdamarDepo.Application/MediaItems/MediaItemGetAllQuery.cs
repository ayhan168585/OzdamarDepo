using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OzdamarDepo.Domain.Abstractions;
using OzdamarDepo.Domain.MediaItems;
using OzdamarDepo.Domain.Users;

namespace OzdamarDepo.Application.MediaItems;

public sealed record MediaItemGetAllQuery() : IRequest<IQueryable<MediaItemGetAllQueryResponse>>;

public sealed class MediaItemGetAllQueryResponse : EntityDto
{
    public string Title { get; set; } = default!;
    public string ImageUrl { get; set; }= default!;
    public string ArtistOrActor { get; set; } = default!;
    public string MediaFormat { get; set; } = default!;
    public string Category { get; set; } = default!;
    public decimal Price { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public int ConditionScore { get; set; }
    public string Description { get; set; }=default!;

    public int MediaDurumValue { get; set; }
    public string MediaDurumName { get; set; } = default!;
    public bool IsBoxSet { get; set; }
    public int DiscCount { get; set; }
}

public sealed class MediaItemsGetAllQueryHandler(IMediaItemRepository mediaItemRepository, UserManager<AppUser> userManager) : IRequestHandler<MediaItemGetAllQuery, IQueryable<MediaItemGetAllQueryResponse>>
{
    public Task<IQueryable<MediaItemGetAllQueryResponse>> Handle(MediaItemGetAllQuery request, CancellationToken cancellationToken)
    {
        var response = (from mediaItem in mediaItemRepository.GetAll().AsNoTracking()

                        join create_user in userManager.Users.AsQueryable()
                            on mediaItem.CreateUserId equals create_user.Id into create_user_join
                        from create_user in create_user_join.DefaultIfEmpty()

                        join update_user in userManager.Users.AsQueryable()
                            on mediaItem.UpdateUserId equals update_user.Id into update_user_join
                        from update_user in update_user_join.DefaultIfEmpty()

                        select new MediaItemGetAllQueryResponse
                        {
                            ArtistOrActor = mediaItem.ArtistOrActor,
                            Category = mediaItem.MediaType.Category,
                            Price = mediaItem.Price,
                            ConditionScore = mediaItem.MediaCondition.ConditionScore,
                            Description = mediaItem.MediaCondition.Description,
                            ReleaseDate = mediaItem.ReleaseDate,
                            IsBoxSet = mediaItem.IsBoxSet,
                            DiscCount = mediaItem.DiscCount,
                            MediaDurumValue = (int)mediaItem.MediaDurum,
                            MediaDurumName = mediaItem.MediaDurum.GetDisplayName(),
                            CreatedAt = mediaItem.CreatedAt,
                            UpdatedAt = mediaItem.UpdatedAt,
                            DeletedAt = mediaItem.DeletedAt,
                            Id = mediaItem.Id,
                            IsDeleted = mediaItem.IsDeleted,
                            MediaFormat = mediaItem.MediaType.Format,
                            Title = mediaItem.Title,
                            ImageUrl = mediaItem.ImageUrl,
                            CreateUserId = mediaItem.CreateUserId,
                            UpdateUserId = mediaItem.UpdateUserId,
                            CreateUserName = create_user == null ? null : create_user.FirstName + " " + create_user.LastName + " (" + create_user.Email + ")",
                            UpdateUserName = update_user == null ? null : update_user.FirstName + " " + update_user.LastName + " (" + update_user.Email + ")"
                        }).AsQueryable();

        return Task.FromResult(response);
    }
}
