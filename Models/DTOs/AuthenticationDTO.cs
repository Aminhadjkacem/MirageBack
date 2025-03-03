using System.ComponentModel.DataAnnotations;

namespace DHP_Manager.Models.DTOs
{
    public class AuthenticationDTO
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
