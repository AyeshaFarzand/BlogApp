using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogApp.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _repository;

        public CommentController(ICommentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var comments = await _repository.GetCommentsAsync();
            return View(comments);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Add(Comment comment)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            comment.UserId = userId;

            await _repository.AddCommentAsync(comment);

            return RedirectToAction("Details", "Post", new { id = comment.PostId }); // Redirect back to post
        }


        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Like(int commentId, int postId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _repository.LikeCommentAsync(commentId, userId);

            return RedirectToAction("Details", "Post", new { id = postId });
        }
    }

}
