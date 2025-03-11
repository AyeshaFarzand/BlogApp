using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    public class UserHomeController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        
    }

}
