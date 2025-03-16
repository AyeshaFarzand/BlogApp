using BlogApp.Models;
using System.Threading.Tasks;

namespace BlogApp.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task<bool> VerifyUserCredentialsAsync(string email, string password);
        Task UpdateUserAsync(User user);
        Task<List<User>> GetAllUsersAsync();
        Task DeleteUserAsync(int id);
    }
}
