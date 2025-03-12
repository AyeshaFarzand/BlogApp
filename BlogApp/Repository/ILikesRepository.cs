using BlogApp.Models;
using System.Threading.Tasks;

namespace BlogApp.Repositories
{
    public interface ILikesRepository
    {
        Task<bool> AddLikeAsync(int postId, int userId);
        Task<bool> HasUserLikedAsync(int postId, int userId);
    }
}
