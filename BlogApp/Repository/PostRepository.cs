using BlogApp.Data;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApp.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;

        public PostRepository(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Get all posts
        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _context.Posts.Include(p => p.User).ToListAsync();
        }

        // ✅ Get a single post by ID
        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await _context.Posts.Include(p => p.User).FirstOrDefaultAsync(p => p.Id == id);
        }

        // ✅ Add a new post
        public async Task AddPostAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        // ✅ Update an existing post
        public async Task UpdatePostAsync(Post post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }

        // ✅ Delete a post
        public async Task DeletePostAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }

        // ✅ Update post status (Pending, Approved, Rejected)
        public async Task UpdatePostStatusAsync(int id, PostStatus status)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                post.Status = status;
                _context.Posts.Update(post);
                await _context.SaveChangesAsync();
            }


           
        }
        public class PostController : Controller
        {
            private readonly AppDbContext _context;

            public PostController(AppDbContext context)
            {
                _context = context;
            }

            // ✅ Show a Post
            public async Task<IActionResult> Show(int id)
            {
                var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
                if (post == null)
                    return NotFound();

                return View(post); // This looks for Views/Post/Show.cshtml
            }
        }



    }
}
