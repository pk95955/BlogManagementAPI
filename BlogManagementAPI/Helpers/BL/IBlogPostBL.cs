using BlogManagementAPI.DTOs.Request;
using BlogManagementAPI.DTOs.Response;
using BlogManagementAPI.Models;

namespace BlogManagementAPI.Helpers.BL
{
    public interface IBlogPostBL
    {
        Task<IEnumerable<BlogPostDTO>> GetAllDataAsync();
        Task<BlogPost> AddPostBlogAsync(BlogPostRequestDTO blogPost, string? userName);
        Task<BlogPost> UpdatePostBlogAsync(BlogPostRequestDTO blogPost);
    }
}
