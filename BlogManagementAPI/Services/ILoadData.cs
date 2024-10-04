using BlogManagementAPI.Models;

namespace BlogManagementAPI.Services
{
    public interface ILoadData
    {
        Task<IEnumerable<BlogPost>> GetAllDataAsync();
        Task<BlogPost?> GetDataByBlogIdAsync(int Id);
        Task<BlogPost> AddPostBlogAsync(BlogPost blogPost);
        Task<BlogPost> UpdatePostBlogAsync(BlogPost blogPost);
        Task<bool> DeleteAsync(int id);
        Task WriteDataAsync();
    }
}
