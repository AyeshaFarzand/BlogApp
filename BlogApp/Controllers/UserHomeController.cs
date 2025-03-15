using BlogApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    public class UserHomeController : Controller
    {
        private readonly IPostRepository _postRepository;

        public UserHomeController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Index()
        {
            var totalPosts = await _postRepository.GetTotalPostsCountAsync();
            var totalLikes = await _postRepository.GetTotalPostLikesCountAsync();
            var totalComments = await _postRepository.GetTotalPostCommentsCountAsync();
            var model = new UserDashboardViewModel
            {
                TotalLikes = totalLikes,
                TotalPosts = totalPosts,
                TotalComments = totalComments
            };

            return View(model);
        }
    }
}
