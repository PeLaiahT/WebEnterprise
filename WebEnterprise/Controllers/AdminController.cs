using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebEnterprise.Data;
using WebEnterprise.Models;

namespace WebEnterprise.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private UserManager<CustomUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AdminController(UserManager<CustomUser> userManager,
            RoleManager<IdentityRole> signInManager, ApplicationDbContext db, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _roleManager = signInManager;
            _webHostEnvironment = env;
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateStaff()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateStaff([Bind("Id, FullName, Address ,UserName, PhoneNumber, ProfileImage")] CustomUser staff)
        {

            
            CustomUser user = await _userManager.FindByNameAsync(staff.UserName);
            if (ModelState.IsValid)
            {
                
                user = new CustomUser();
                user.UserName = staff.UserName;
                user.Email = staff.Email;
                user.ImageName = staff.ImageName;
                user.PhoneNumber = staff.PhoneNumber;
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(staff.ProfileImage.FileName);
                string extension = Path.GetExtension(staff.ProfileImage.FileName);
                user.ImageName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Img/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await staff.ProfileImage.CopyToAsync(fileStream);
                }
                IdentityResult result = await _userManager.CreateAsync(user, "Staff123!");
                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, "Staff").Wait();
                }
                ViewBag.Message = "User was created";
                return RedirectToAction("Index");
            }
            return View(staff);
        }
    }
}
