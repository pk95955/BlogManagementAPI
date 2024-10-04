namespace BlogManagementAPI.Models
{
    public class BlogPost
    {
        public int Id { get; set; }

        public string? UserName { get; set; }
        public DateTime DateCreate { get; set; }

        public string? PostText { get; set; }
    }
}
