using BlogApp.Data;
using BlogApp.Models;

using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace BlogApp.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddCommentAsync(Comment comment)
        {
            comment.CreatedAt = DateTime.Now;
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Comment>> GetCommentsAsync()
        {
            return await _context.Comments
                .Include(c => c.Likes)
                .Include(c => c.Replies)
                .ToListAsync();
        }

        public async Task LikeCommentAsync(int commentId, int userId)
        {
            var like = new CommentLike
            {
                CommentId = commentId,
               UserId = userId
            };
            _context.CommentLikes.Add(like);
            await _context.SaveChangesAsync();
        }
    }

}
