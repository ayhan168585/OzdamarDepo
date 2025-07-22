namespace OzdamarDepo.Application.Auth
{
    public sealed record LoginCommandResponse
    {
        public string AccessToken { get; set; } = default!;
        public UserDto User { get; set; } = default!;
    }

    public sealed class UserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;

        // ✅ Ödeme için gerekli yeni alanlar:
        public string TcNo { get; set; } = default!;
        public string Phone { get; set; } = default!;
    }
}
