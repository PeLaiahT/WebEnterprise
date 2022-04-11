using Microsoft.AspNetCore.Mvc;

namespace WebEnterprise.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
