namespace BlogManagementAPI.DTOs.Response
{
    public class BlogPostDTO
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public DateTime DateCreate { get; set; }
        public string? PostText { get; set; }
    }
}
