using Microsoft.AspNetCore.Mvc;
using WebEnterprise.Data;
using WebEnterprise.Models;

namespace WebEnterprise.Controllers
{
    public class IdeaController : Controller
    {
        private readonly ApplicationDbContext _db;
        public IdeaController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var ideaList = _db.Ideas.ToList();
            return View(ideaList);
        }
       

    }
}
