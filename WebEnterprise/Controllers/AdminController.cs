
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebEnterprise.Data;
using WebEnterprise.Models;
using WebEnterprise.Models.DTO;

namespace WebEnterprise.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private UserManager<CustomUser> _userManager;

        public AdminController(UserManager<CustomUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        private List<SelectListItem> GetDropDownDepartment()
        {
            var departments = _db.Departments.Select(c => new SelectListItem { Text = c.NameDepartment, Value = c.DepartmentID.ToString() }).ToList();
            return departments;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var admins = (from u in _db.Users
                          join ur in _db.UserRoles on u.Id equals ur.UserId
                          join r in _db.Roles on ur.RoleId equals r.Id
                          where r.Name == "Admin"
                          select new CustomUserDTO
                          {
                              Id = u.Id,
                              UserName = u.UserName,
                              Email = u.Email,
                              PhoneNumber = u.PhoneNumber,
                          }
                          ).ToList();
            return View(admins);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ViewAllStaff()
        {
            var staffs = (from u in _db.CustomUsers
                          join ur in _db.UserRoles on u.Id equals ur.UserId
                          join r in _db.Roles on ur.RoleId equals r.Id
                          join d in _db.Departments on u.DepartmentID equals d.DepartmentID
                          where r.Name == "Staff"
                          select new CustomUserDTO
                          {
                              Id = u.Id,
                              UserName = u.UserName,
                              Email = u.Email,
                              PhoneNumber = u.PhoneNumber,
                              FullName = u.FullName,
                              FileName = u.FileName,
                              Department = _db.Departments.FirstOrDefault(d => d.DepartmentID == d.DepartmentID),
                          }
                          ).ToList();
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
            if (ModelState.IsValid)
            {
                foreach (IFormFile f in postedFile)
                {
                    using (var dataStream = new MemoryStream())
                    {
                        await f.CopyToAsync(dataStream);
                        staff.Image = dataStream.ToArray();
                    }
                    if (postedFile != null)
                    {
                        //chi dinh duong dan se luu
                        string fullPath = Path.Combine(Directory.GetCurrentDirectory(),
                            "wwwroot", "Img", f.FileName);
                        staff.FileName = f.FileName;
                        using (var file = new FileStream(fullPath, FileMode.Create))
                        {
                            f.CopyTo(file);
                        }
                    }
                }
                var user = new CustomUser
                {
                    UserName = staff.UserName,
                    FullName = staff.FullName,
                    Email = staff.Email,
                    PhoneNumber = staff.PhoneNumber,
                    Image = staff.Image,
                    FileName = staff.FileName,
                    DepartmentID = staff.DepartmentID
                };
                var result = await _userManager.CreateAsync(user, "Staff123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Staff");
                }
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
            var staff = _db.Users.FirstOrDefault(u => u.Id == id);
            if (staff == null)
            {
                return RedirectToAction("Index");
            }
            _db.Remove(staff);
            _db.SaveChanges();
            return RedirectToAction("ViewAllStaff");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult EditStaff(string id)
        {
            ViewBag.departments = GetDropDownDepartment();
            var staff = _db.CustomUsers.Where(s => s.Id == id).
                Select(u => new CustomUserDTO
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    FullName = u.FullName,
                    Department = u.Department,
                    FileName = u .FileName,
                }).FirstOrDefault();
            if (staff == null)
            {
                return RedirectToAction("Index");
            }
            return View(staff);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult EditStaff(CustomUserDTO staff)
        {

            if (ModelState.IsValid)
            {
                var newstaff = _db.CustomUsers.Find(staff.Id);
                if (newstaff == null)
                {
                    ViewBag.departments = GetDropDownDepartment();
                    return View(staff);
                }
                else
                {
                    newstaff.UserName = staff.UserName;
                    newstaff.Email = staff.Email;
                    newstaff.PhoneNumber = staff.PhoneNumber;
                    newstaff.FullName = staff.FullName;
                    newstaff.Department = staff.Department;
                    _db.SaveChanges();
                }
                return RedirectToAction("ViewAllStaff");
            }
            return View(staff);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ViewAllCoordinator()
        {
            var coor = (from u in _db.CustomUsers
                        join ur in _db.UserRoles on u.Id equals ur.UserId
                        join r in _db.Roles on ur.RoleId equals r.Id
                        where r.Name == "Coordinator"
                        select new CustomUserDTO
                        {
                            Id = u.Id,
                            UserName = u.UserName,
                            Email = u.Email,
                            PhoneNumber = u.PhoneNumber,
                            FullName = u.FullName,
                            FileName = u.FileName,
                            Department = _db.Departments.FirstOrDefault(d => d.DepartmentID == d.DepartmentID),
                        }
                          ).ToList();
            return View(coor);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult CreateCoor()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCoor(CustomUserDTO coor, List<IFormFile> postedFile)
        {
            if (ModelState.IsValid)
            {
                foreach (IFormFile f in postedFile)
                {
                    using (var dataStream = new MemoryStream())
                    {
                        await f.CopyToAsync(dataStream);
                        coor.Image = dataStream.ToArray();
                    }
                    if (postedFile != null)
                    {
                        //chi dinh duong dan se luu
                        string fullPath = Path.Combine(Directory.GetCurrentDirectory(),
                            "wwwroot", "Img", f.FileName);
                        ViewBag.fileName = f.FileName;
                        using (var file = new FileStream(fullPath, FileMode.Create))
                        {
                            f.CopyTo(file);
                        }
                    }
                }
                var user = new CustomUser
                {
                    UserName = coor.UserName,
                    FullName = coor.FullName,
                    Email = coor.Email,
                    PhoneNumber = coor.PhoneNumber,
                };
                var result = await _userManager.CreateAsync(user, "Coor123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Coordinator");
                }
                return RedirectToAction("ViewAllCoordinator");
            }
            return View("ViewAllCoordinator");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCoor(string id)
        {
            var coor = _db.Users.FirstOrDefault(u => u.Id == id);
            if (coor == null)
            {
                return RedirectToAction("ViewAllCoordinator");
            }
            _db.Remove(coor);
            _db.SaveChanges();
            return RedirectToAction("ViewAllCoordinator");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult EditCoor(string id)
        {
            var coor = _db.CustomUsers.Where(s => s.Id == id).
                Select(u => new CustomUserDTO
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    FullName = u.FullName
                }).FirstOrDefault();
            if (coor == null)
            {
                return RedirectToAction("ViewAllCoordinator");
            }
            return View(coor);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult EditCoor(CustomUserDTO staff)
        {
            if (ModelState.IsValid)
            {
                var newcoor = _db.CustomUsers.Find(staff.Id);
                if (newcoor == null)
                {
                    return View(staff);
                }
                else
                {
                    newcoor.UserName = staff.UserName;
                    newcoor.Email = staff.Email;
                    newcoor.PhoneNumber = staff.PhoneNumber;
                    newcoor.FullName = staff.FullName;
                    _db.SaveChanges();
                }
                return RedirectToAction("ViewAllCoordinator");
            }
            return View(staff);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ViewAllManager()
        {
            var manager = (from u in _db.CustomUsers
                           join ur in _db.UserRoles on u.Id equals ur.UserId
                           join r in _db.Roles on ur.RoleId equals r.Id
                           where r.Name == "Assurance"
                           select new CustomUserDTO
                           {
                               Id = u.Id,
                               UserName = u.UserName,
                               Email = u.Email,
                               PhoneNumber = u.PhoneNumber,
                               FullName = u.FullName,
                               FileName = u.FileName,
                               Department = _db.Departments.FirstOrDefault(d => d.DepartmentID == d.DepartmentID),
                           }
                          ).ToList();
            return View(manager);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult CreateManager()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateManager(CustomUserDTO coor, List<IFormFile> postedFile)
        {
            if (ModelState.IsValid)
            {
                foreach (IFormFile f in postedFile)
                {

                    using (var dataStream = new MemoryStream())
                    {
                        await f.CopyToAsync(dataStream);
                        coor.Image = dataStream.ToArray();
                    }
                    if (postedFile != null)
                    {
                        //chi dinh duong dan se luu
                        string fullPath = Path.Combine(Directory.GetCurrentDirectory(),
                            "wwwroot", "Img", f.FileName);
                        ViewBag.fileName = f.FileName;
                        using (var file = new FileStream(fullPath, FileMode.Create))
                        {
                            f.CopyTo(file);
                        }
                    }
                }
                var user = new CustomUser
                {
                    UserName = coor.UserName,
                    FullName = coor.FullName,
                    Email = coor.Email,
                    PhoneNumber = coor.PhoneNumber,
                };
                var result = await _userManager.CreateAsync(user, "Manager123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Assurance");
                }
                return RedirectToAction("ViewAllManager");
            }
            return View("ViewAllManager");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteManager(string id)
        {
            var manager = _db.Users.FirstOrDefault(u => u.Id == id);
            if (manager == null)
            {
                return RedirectToAction("ViewAllManager");
            }
            _db.Remove(manager);
            _db.SaveChanges();
            return RedirectToAction("ViewAllManager");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult EditManager(string id)
        {
            var manager = _db.CustomUsers.Where(s => s.Id == id).
                Select(u => new CustomUserDTO
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    FullName = u.FullName
                }).FirstOrDefault();
            if (manager == null)
            {
                return RedirectToAction("ViewAllManager");
            }
            return View(manager);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult EditManager(CustomUserDTO staff)
        {
            if (ModelState.IsValid)
            {
                var newManager = _db.CustomUsers.Find(staff.Id);
                if (newManager == null)
                {
                    return View(staff);
                }
                else
                {
                    newManager.UserName = staff.UserName;
                    newManager.Email = staff.Email;
                    newManager.PhoneNumber = staff.PhoneNumber;
                    newManager.FullName = staff.FullName;
                    _db.SaveChanges();
                }
                return RedirectToAction("ViewAllManager");
            }
            return View(staff);
        }
    }
}
