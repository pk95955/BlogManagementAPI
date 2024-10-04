using System.ComponentModel.DataAnnotations;

namespace BlogManagementAPI.DTOs.Request
{
    public class BlogPostRequestDTO
    {
        public int Id { get; set; }
        [Required]
        public string? PostText { get; set; }
    }
}
