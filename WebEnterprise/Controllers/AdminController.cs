
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
                          where r.Name == "Staff"
                          join d in _db.Departments on u.DepartmentID equals d.DepartmentID
                          select new CustomUserDTO
                          {
                              Id = u.Id,
                              UserName = u.UserName,
                              Email = u.Email,
                              Address = u.Address,
                              PhoneNumber = u.PhoneNumber,
                              FullName = u.FullName,
                              FileName = u.FileName,
                              Department = _db.Departments.FirstOrDefault(s => s.DepartmentID == d.DepartmentID),
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
            IdeaValidation(staff);
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
                    Address = staff.Address,
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
            var staffs = _db.CustomUsers.FirstOrDefault(u => u.Id == id);
            var ideas = _db.Ideas.Where(i => i.IdeaUserID == id).ToList();
            var comments = _db.Comments.Where(c => c.CommentUserID == id).ToList();
            var likes = _db.Likes.Where(l => l.LikeUserID == id).ToList();
            if (staffs == null)
            {
                return RedirectToAction("Index");
            }
            _db.CustomUsers.Remove(staffs);
            foreach (var i in ideas)
            {
                _db.Ideas.Remove(i);
            }
            foreach (var c in comments)
            {
                _db.Comments.Remove(c);
            }
            foreach (var l in likes)
            {
                _db.Likes.Remove(l);
            }
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
                    Address = u.Address,
                    PhoneNumber = u.PhoneNumber,
                    FullName = u.FullName,
                    Department = u.Department,
                    FileName = u.FileName,
                }).FirstOrDefault();
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
                var newstaff = _db.CustomUsers.Find(staff.Id);
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
                if(postedFile.Count == 0)
                {
                   
                        staff.FileName = newstaff.FileName;
                    
                }    
                if (newstaff == null)
                {
                    ViewBag.departments = GetDropDownDepartment();
                    return View(staff);
                }
                else
                {
                    newstaff.UserName = staff.UserName;
                    newstaff.Email = staff.Email;
                    newstaff.Address = staff.Address;
                    newstaff.PhoneNumber = staff.PhoneNumber;
                    newstaff.FullName = staff.FullName;
                    newstaff.Department = staff.Department;
                    newstaff.Image = staff.Image;
                    newstaff.FileName = staff.FileName;
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
                        coor.FileName = f.FileName;
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
                    Image = coor.Image,
                    FileName = coor.FileName,
                    DepartmentID = coor.DepartmentID
                };
                var result = await _userManager.CreateAsync(user, "Coor123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Coordinator");
                }
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
        public async Task<IActionResult> EditCoor(CustomUserDTO coor, List<IFormFile> postedFile)
        {
            IdeaValidation(coor);
            if (ModelState.IsValid)
            {
                var newcoor = _db.CustomUsers.Find(coor.Id);
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
                        coor.FileName = f.FileName;
                        using (var file = new FileStream(fullPath, FileMode.Create))
                        {
                            f.CopyTo(file);
                        }
                    }
                }
                if (postedFile.Count == 0)
                {
                    coor.FileName = newcoor.FileName;
                }
                if (newcoor == null)
                {
                    return View(coor);
                }
                else
                {
                    newcoor.UserName = coor.UserName;
                    newcoor.Email = coor.Email;
                    newcoor.PhoneNumber = coor.PhoneNumber;
                    newcoor.FullName = coor.FullName;
                    _db.SaveChanges();
                }
                return RedirectToAction("ViewAllCoordinator");
            }
            return View(coor);
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
        public async Task<IActionResult> CreateManager(CustomUserDTO manager, List<IFormFile> postedFile)
        {
            IdeaValidation(manager);
            if (ModelState.IsValid)
            {
                foreach (IFormFile f in postedFile)
                {
                    using (var dataStream = new MemoryStream())
                    {
                        await f.CopyToAsync(dataStream);
                        manager.Image = dataStream.ToArray();
                    }
                    if (postedFile != null)
                    {
                        //chi dinh duong dan se luu
                        string fullPath = Path.Combine(Directory.GetCurrentDirectory(),
                            "wwwroot", "Img", f.FileName);
                        manager.FileName = f.FileName;
                        using (var file = new FileStream(fullPath, FileMode.Create))
                        {
                            f.CopyTo(file);
                        }
                    }
                }
                var user = new CustomUser
                {
                    UserName = manager.UserName,
                    FullName = manager.FullName,
                    Email = manager.Email,
                    PhoneNumber = manager.PhoneNumber,
                    Image = manager.Image,
                    FileName = manager.FileName
                };
                var result = await _userManager.CreateAsync(user, "Manager123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Assurance");
                }
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
        public async Task<IActionResult> EditManager(CustomUserDTO manager, List<IFormFile> postedFile)
        {
            IdeaValidation(manager);
            if (ModelState.IsValid)
            {
                var newManager = _db.CustomUsers.Find(manager.Id);
                foreach (IFormFile f in postedFile)
                {
                    if (postedFile != null)
                    {
                        //chi dinh duong dan se luu
                        string fullPath = Path.Combine(Directory.GetCurrentDirectory(),
                            "wwwroot", "Img", f.FileName);
                        manager.FileName = f.FileName;
                        using (var file = new FileStream(fullPath, FileMode.Create))
                        {
                            f.CopyTo(file);
                        }
                    }
                }
                if (postedFile.Count == 0)
                {
                    manager.FileName = newManager.FileName;
                }
                if (newManager == null)
                {
                    return View(manager);
                }
                else
                {
                    newManager.UserName = manager.UserName;
                    newManager.Email = manager.Email;
                    newManager.PhoneNumber = manager.PhoneNumber;
                    newManager.FullName = manager.FullName;
                    newManager.FileName = manager.FileName;

                    _db.SaveChanges();
                }
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
            if (string.IsNullOrEmpty(person.Address))
            {
                ModelState.AddModelError("Address", "Please Input address");
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
