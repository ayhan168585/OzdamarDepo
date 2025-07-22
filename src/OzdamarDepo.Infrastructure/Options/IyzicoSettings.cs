using System.ComponentModel.DataAnnotations;

namespace OzdamarDepo.Infrastructure.Options
{
   
        
        public class IyzicoSettings
        {
            [Required]
            public string ApiKey { get; set; } = string.Empty;

            [Required]
            public string SecretKey { get; set; } = string.Empty;

            [Required]
            public string BaseUrl { get; set; } = string.Empty;
        }
    
    
}
