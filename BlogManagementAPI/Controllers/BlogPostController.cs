using BlogManagementAPI.DTOs.Request;
using BlogManagementAPI.DTOs.Response;
using BlogManagementAPI.Helpers.BL;
using BlogManagementAPI.Models;
using BlogManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BlogPostController : ControllerBase
    {
        private readonly ILoadData _loadData;
        private readonly IBlogPostBL _postBL;

        public BlogPostController(ILoadData loadData, IBlogPostBL postBL)
        {
            _loadData = loadData;
            _postBL = postBL;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<IEnumerable<BlogPostDTO>>> GetAll()
        {
            var postData = await _postBL.GetAllDataAsync();
            return Ok(postData);
        }

        // GET 
        [HttpGet("{id}")]
        public async Task<ActionResult<BlogPost>> GetById(int id)
        {
            var post = await _loadData.GetDataByBlogIdAsync(id);
            if (post == null)
                return NotFound();
            return Ok(post);
        }

        // POST
        [HttpPost("Post")]
        public async Task<ActionResult<BlogPost>> Create(BlogPostRequestDTO blogPostDTO)
        {
            var userId = User.FindFirst("id")?.Value;
            var username = User.Identity?.Name;
            var createdPost = await _postBL.AddPostBlogAsync(blogPostDTO, username);
            return CreatedAtAction(nameof(GetById), new { id = createdPost.Id }, createdPost);
        }
        [HttpPost("update")]
        public async Task<ActionResult<BlogPost>> Update(BlogPostRequestDTO blogPostDTO)
        {
            var userId = User.FindFirst("id")?.Value;
            var username = User.Identity?.Name;
            var createdPostData = await _postBL.UpdatePostBlogAsync(blogPostDTO);
            return CreatedAtAction(nameof(GetById), new { id = createdPostData.Id }, createdPostData);
        }


        // DELETE 
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _loadData.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

    }
}
