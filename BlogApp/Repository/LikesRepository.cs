using BlogApp.Data;
using BlogApp.Models;

using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BlogApp.Repositories
{
    public class LikesRepository : ILikesRepository
    {
        private readonly AppDbContext _context;

        public LikesRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> HasUserLikedAsync(int postId, int userId)
        {
            return await _context.Likes.AnyAsync(l => l.PostId == postId && l.UserId == userId);
        }

        public async Task<bool> AddLikeAsync(int postId, int userId)
        {
            if (!await HasUserLikedAsync(postId, userId))
            {
                var like = new Like
                {
                    PostId = postId,
                    UserId = userId
                };

                _context.Likes.Add(like);
                await _context.SaveChangesAsync();
                return true;
            }

            return false; // already liked
        }
        public async Task<int> GetLikeCountForPostAsync(int postId)
        {
            return await _context.Likes.CountAsync(l => l.PostId == postId);
        }

    }
}
