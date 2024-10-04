using AutoMapper;
using BlogManagementAPI.DTOs.Request;
using BlogManagementAPI.DTOs.Response;
using BlogManagementAPI.Models;
using BlogManagementAPI.Services;

namespace BlogManagementAPI.Helpers.BL
{
    public class BlogPostBL : IBlogPostBL
    {
        private readonly ILoadData _loadData;
        private readonly IMapper _mapper;
        public BlogPostBL(ILoadData loadData, IMapper mapper)
        {
            _loadData = loadData;
            _mapper = mapper;
        }

        public Task<IEnumerable<BlogPostDTO>> GetAllDataAsync()
        {
            var blogPosts = _loadData.GetAllDataAsync().Result;
            var blogPostDTO = _mapper.Map<IEnumerable<BlogPostDTO>>(blogPosts);
            return Task.FromResult(blogPostDTO);
        }
        public Task<BlogPost> AddPostBlogAsync(BlogPostRequestDTO blogPostDTO, string? userName)
        {
            var blogPostRequest = _mapper.Map<BlogPost>(blogPostDTO);
            blogPostRequest.UserName = userName;
            var blogPostResponse = _loadData.AddPostBlogAsync(blogPostRequest).Result;
            return Task.FromResult(blogPostResponse);
        }
        public Task<BlogPost> UpdatePostBlogAsync(BlogPostRequestDTO blogPostDTO)
        {
            var blogPostRequest = _mapper.Map<BlogPost>(blogPostDTO);
            var blogPostResponse = _loadData.UpdatePostBlogAsync(blogPostRequest).Result;
            return Task.FromResult(blogPostResponse);
        }
    }
}
