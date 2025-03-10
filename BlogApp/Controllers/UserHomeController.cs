using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    public class UserHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        

    }

}
