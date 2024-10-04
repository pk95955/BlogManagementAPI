using BlogManagementAPI.Models;
using System.Text.Json;

namespace BlogManagementAPI.Services
{
    public class UserServices : IUserService
    {
        private readonly string _jsonFilePath;
        private List<User> _users;
        private readonly object _lock = new();

        public UserServices(IWebHostEnvironment env)
        {
            _users = new();
            //JSON file is placed in the Data folder
            _jsonFilePath = Path.Combine(env.ContentRootPath, "Data", "UserData.json");
            LoadUserData();
        }
        public Task<User?> ValidateUser(string username, string password)
        {            
            return Task.FromResult(_users.Find(u => u.Username == username && u.Password == password));
        }
        public void LoadUserData()
        {
            if (File.Exists(_jsonFilePath))
            {
                var json = File.ReadAllText(_jsonFilePath);
                _users = JsonSerializer.Deserialize<List<User>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<User>();
            }
            else
            {
                _users = new List<User>();
            }
        }
    }
}
