using Microsoft.AspNetCore.Mvc;
using WebEnterprise.Models;

namespace WebEnterprise.Controllers
{
    public class IdeaController : Controller
    {
        
        public IActionResult Index()
        {
            var idea1 = new List <Idea>();
            var idea2 = new Idea();
            var idea3 = new Idea();
            idea1.Add(idea2);
            idea1.Add(idea3);
            return View(idea1);
        }
       

    }
}
