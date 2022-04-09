
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebEnterprise.Data;
using WebEnterprise.Models;
using WebEnterprise.Models.DTO;
using WebEnterprise.Respon;

namespace WebEnterprise.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private UserManager<CustomUser> _userManager;
        private readonly IAdminRespon adminRespon;

        public AdminController(UserManager<CustomUser> userManager, ApplicationDbContext db,IAdminRespon _adminRespon)
        {
            _userManager = userManager;
            _db = db;
            adminRespon = _adminRespon;
        }
        private List<SelectListItem> GetDropDownDepartment()
        {
            var departments = _db.Departments.Select(c => new SelectListItem { Text = c.NameDepartment, Value = c.DepartmentID.ToString() }).ToList();
            return departments;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var admins = adminRespon.GetAllAdmin();
            return View(admins);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ViewAllStaff()
        {
            var staffs = adminRespon.GetAllStaff();
            return View(staffs);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult CreateStaff()
        {
            ViewBag.departments = GetDropDownDepartment();
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateStaff(CustomUserDTO staff, List<IFormFile> postedFile)
        {
            IdeaValidation(staff);
            if (ModelState.IsValid)
            {
                var staff1 = await adminRespon.PostCreateStaff(staff, postedFile);
                return RedirectToAction("ViewAllStaff");
            }
            else
            {
                ViewBag.departments = GetDropDownDepartment();
                return View(staff);
            }

        }
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteStaff(string id)
        {
            var staff1 = adminRespon.DeleteStaff(id);
            return RedirectToAction("ViewAllStaff");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult EditStaff(string id)
        {
            ViewBag.departments = GetDropDownDepartment();
            var staff = adminRespon.GetEditStaff(id);
            if (staff == null)
            {
                return RedirectToAction("Index");
            }
            return View(staff);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditStaff(CustomUserDTO staff, List<IFormFile> postedFile)
        {
            IdeaValidation(staff);
            if (ModelState.IsValid)
            {
               var staff1 = await adminRespon.PostEditStaff(staff, postedFile);
                return RedirectToAction("ViewAllStaff");
            }
            return View(staff);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ViewAllCoordinator()
        {
            var coor = adminRespon.GetAllCoor();
            return View(coor);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult CreateCoor()
        {
            ViewBag.departments = GetDropDownDepartment();
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCoor(CustomUserDTO coor, List<IFormFile> postedFile)
        {
            IdeaValidation(coor);
            if (ModelState.IsValid)
            {
                var cor = await adminRespon.PostCreateCoor(coor,postedFile);
                return RedirectToAction("ViewAllCoordinator");
            }
            else
            {
                ViewBag.departments = GetDropDownDepartment();
                return View(coor);
            }
        }
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCoor(string id)
        {
            var coor = adminRespon.DeleteCoor(id);
            return RedirectToAction("ViewAllCoordinator");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult EditCoor(string id)
        {
            var coor = adminRespon.GetEditCoor(id);
            if (coor == null)
            {
                return RedirectToAction("ViewAllCoordinator");
            }
            return View(coor);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditCoor(CustomUserDTO coor, List<IFormFile> postedFile)
        {
            IdeaValidation(coor);
            if (ModelState.IsValid)
            {
                var cor = await adminRespon.PostEditCoor(coor, postedFile);
                return RedirectToAction("ViewAllCoordinator");
            }
            return View(coor);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ViewAllManager()
        {
            var manager = adminRespon.GetAllManager();
            return View(manager);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult CreateManager()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateManager(CustomUserDTO manager, List<IFormFile> postedFile)
        {
            IdeaValidation(manager);
            if (ModelState.IsValid)
            {
                var man = await adminRespon.PostCreateManager(manager, postedFile);
                return RedirectToAction("ViewAllManager");
            }
            else
            {
                return View(manager);
            }
        }
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteManager(string id)
        {
            var manager = adminRespon.DeleteManager(id);
            return RedirectToAction("ViewAllManager");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult EditManager(string id)
        {
            var manager = adminRespon.GetEditManager(id);
            if (manager == null)
            {
                return RedirectToAction("ViewAllManager");
            }
            return View(manager);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditManager(CustomUserDTO manager, List<IFormFile> postedFile)
        {
            IdeaValidation(manager);
            if (ModelState.IsValid)
            {
                var man = await adminRespon.PostEditManager(manager, postedFile);
                return RedirectToAction("ViewAllManager");
            }
            return View(manager);
        }
        private void IdeaValidation(CustomUserDTO person)
        {
            if (string.IsNullOrEmpty(person.Email))
            {
                ModelState.AddModelError("Email", "Please Input Email");
            }
            if (string.IsNullOrEmpty(person.UserName))
            {
                ModelState.AddModelError("Name", "Please Input userName");
            }
            if (string.IsNullOrEmpty(person.PhoneNumber) )
            {
                ModelState.AddModelError("Phone", "Please Input Phone Number");
            }
            if(string.IsNullOrEmpty(person.FullName))
                
            {
                ModelState.AddModelError("FullName", "Please Input Fullname");
            }
            if (!person.PhoneNumber.StartsWith("0"))
            {
                ModelState.AddModelError("Phone", "You should input correct phone Number");
            }    
            if(person.PhoneNumber.Length > 11 && person.PhoneNumber.Length<10)
            {
                ModelState.AddModelError("Phone", "You input incorrect Phone Number");
            }
            

           

        }
    }

}
