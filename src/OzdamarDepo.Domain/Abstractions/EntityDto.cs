namespace OzdamarDepo.Domain.Abstractions;

public abstract class EntityDto
{
    public Guid Id { get; set; }
    public Guid CreateUserId { get; set; }
    public string CreateUserName { get; set; } = default!;
    public Guid? UpdateUserId { get; set; }
    public string? UpdateUserName { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset DeletedAt { get; set; }
} 