using BlogManagementAPI.Helpers.BL;
using BlogManagementAPI.Models;
using System.Text.Json;

namespace BlogManagementAPI.Services
{
    public class LoadData : ILoadData, IHostedService
    {
        private readonly string _jsonFilePath;
        private List<BlogPost>? _blogPost ;
        private readonly object _lock = new();
        private readonly Timer _timer;
        public LoadData(IWebHostEnvironment env)
        {
             
            // JSON file is placed in the Data folder
            _jsonFilePath = Path.Combine(env.ContentRootPath, "Data", "blogpostdata.json");
            LoadBlogData();
            // Set up a timer to persist data every 5 minutes
            _timer = new Timer(async _ => await WriteDataAsync(), null, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
        }
        // Implement IHostedService to hook into application lifetime
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // No initialization required here
            return Task.CompletedTask;
        }
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // Persist data when the application is stopping
            await WriteDataAsync();
        }
        private void LoadBlogData()
        {
            if (File.Exists(_jsonFilePath))
            {
                var json = File.ReadAllText(_jsonFilePath);
                _blogPost = JsonSerializer.Deserialize<List<BlogPost>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<BlogPost>();
            }
            else
            {
                _blogPost = new List<BlogPost>();
            }
        }
        public Task<IEnumerable<BlogPost>> GetAllDataAsync()
        {
            if(_blogPost!=null)
            return Task.FromResult(_blogPost.AsEnumerable());
            else
                return Task.FromResult<IEnumerable<BlogPost>>(new List<BlogPost>());

        }
        public Task<BlogPost?> GetDataByBlogIdAsync(int Id)
        {
            var post = _blogPost?.FirstOrDefault(p => p.Id == Id);
            return Task.FromResult(post);
        }
        public Task<BlogPost> AddPostBlogAsync(BlogPost blogPost)
        {
            lock (_lock)
            {
                // Generate a new Id 
                blogPost.DateCreate = DateTime.Now;
                if (_blogPost != null)
                {
                    blogPost.Id = _blogPost.Any() ? _blogPost.Max(p => p.Id) + 1 : 1;
                    _blogPost.Add(blogPost);
                }
            }
            return Task.FromResult(blogPost);
        }
        public Task<BlogPost> UpdatePostBlogAsync(BlogPost blogPost)
        {
            lock (_lock)
            {
                var post = _blogPost?.FirstOrDefault(p => p.Id == blogPost.Id);
                if(post!=null)
                post.PostText = blogPost.PostText;
            }
            return Task.FromResult(blogPost);
        }
        public Task<bool> DeleteAsync(int id)
        {
            lock (_lock)
            {
                var postData = _blogPost?.FirstOrDefault(p => p.Id == id);
                if (postData == null)
                {
                    return Task.FromResult(false);
                }
                _blogPost?.Remove(postData);
            }
            return Task.FromResult(true);
        }
        public Task WriteDataAsync()
        {
            lock (_lock)
            {
                var json = JsonSerializer.Serialize(_blogPost, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_jsonFilePath, json);
            }
            return Task.CompletedTask;
        }

    }
}
