using BlogApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class AdminHomeController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;

        public AdminHomeController(IUserRepository userRepository,IPostRepository postRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
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
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();

            var userViewModels = users.Select(u => new UserViewModel
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                RoleName = u.Role.RoleName
            }).ToList();

            return View(userViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userRepository.DeleteUserAsync(id);
            return RedirectToAction("ManageUsers");
        }
    }

}

