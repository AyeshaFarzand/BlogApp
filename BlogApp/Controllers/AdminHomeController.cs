using BlogApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class AdminHomeController : Controller
    {
        private readonly IPostRepository _postRepository;

        public AdminHomeController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var totalPosts = await _postRepository.GetTotalPostsCountAsync();
            //   var totalLikes = await _postRepository.GetTotalPostLikesCountAsync();
            //   var totalComments = await _postRepository.GetTotalPostCommentsCountAsync();
            //var totalApproved = await _postRepository.GetTotalApprovePostsCountAsync();
            var model = new UserDashboardViewModel
            {
            //    TotalLikes = totalLikes,
                TotalPosts = totalPosts,
               
                //    TotalComments = totalComments
            };

            return View(model);
        }

    }
}
