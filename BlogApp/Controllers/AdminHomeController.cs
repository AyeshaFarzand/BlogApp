using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    public class AdminHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
