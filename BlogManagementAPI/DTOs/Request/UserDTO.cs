using System.ComponentModel.DataAnnotations;

namespace BlogManagementAPI.DTOs.Request
{
    public class UserDTO
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
