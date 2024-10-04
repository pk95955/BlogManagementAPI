using BlogManagementAPI.Models;

namespace BlogManagementAPI.Services
{
    public interface IUserService
    {
        Task<User?> ValidateUser(string? userName, string? password);
    }
}
