using Microsoft.AspNetCore.Mvc;
using WebEnterprise.Models;

namespace WebEnterprise.Controllers
{
    public class IdeaController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }

    }
}
