namespace OzdamarDepo.Application.Auth.Dtos
{

    public sealed class LoginResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Ödeme için gereken ek alanlar
        public string? TcNo { get; set; }
        public string? Phone { get; set; }
    }


    public class LoginResultDto
    {
        public LoginResponseDto User { get; set; } = null!;
        public string AccessToken { get; set; } = string.Empty;
    }
}
