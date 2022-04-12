using Microsoft.AspNetCore.Identity;
using WebEnterprise.Data;
using WebEnterprise.Models;
using WebEnterprise.Models.DTO;

namespace WebEnterprise.Respon
{
    public class AdminRepo : IAdminRespon
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<CustomUser> userManager;
        public AdminRepo(ApplicationDbContext _context, UserManager<CustomUser> _userManager)
        {
            context = _context;
            userManager = _userManager;

        }
        public bool DeleteManager(string id)
        {
            var manager = context.Users.FirstOrDefault(u => u.Id == id);
            if (manager != null)
            {
                context.Remove(manager);
                context.SaveChanges();
                return true;
            }
            return false;
        }



        public List<CustomUserDTO> GetAllAdmin()
        {
            var admins = (from u in context.Users
                          join ur in context.UserRoles on u.Id equals ur.UserId
                          join r in context.Roles on ur.RoleId equals r.Id
                          where r.Name == "Admin"
                          select new CustomUserDTO
                          {
                              Id = u.Id,
                              UserName = u.UserName,
                              Email = u.Email,
                              PhoneNumber = u.PhoneNumber,
                          }
                          ).ToList();
            if(admins != null)
            {
                return admins;
            }
            return null;

        }

        public List<CustomUserDTO> GetAllCoor()
        {
            var coor = (from u in context.CustomUsers
                        join ur in context.UserRoles on u.Id equals ur.UserId
                        join r in context.Roles on ur.RoleId equals r.Id
                        where r.Name == "Coordinator"
                        select new CustomUserDTO
                        {
                            Id = u.Id,
                            UserName = u.UserName,
                            Email = u.Email,
                            PhoneNumber = u.PhoneNumber,
                            FullName = u.FullName,
                            FileName = u.FileName,
                            Department = context.Departments.FirstOrDefault(d => d.DepartmentID == d.DepartmentID),
                        }
                          ).ToList();
            if (coor != null)
            {
                return coor;
            }
            return null;

        }

        public List<CustomUserDTO> GetAllManager()
        {
            var manager = (from u in context.CustomUsers
                           join ur in context.UserRoles on u.Id equals ur.UserId
                           join r in context.Roles on ur.RoleId equals r.Id
                           where r.Name == "Assurance"
                           select new CustomUserDTO
                           {
                               Id = u.Id,
                               UserName = u.UserName,
                               Email = u.Email,
                               PhoneNumber = u.PhoneNumber,
                               FullName = u.FullName,
                               FileName = u.FileName,
                               Department = context.Departments.FirstOrDefault(d => d.DepartmentID == d.DepartmentID),
                           }
                      ).ToList();
            if(manager != null)
            {
                return manager;
            }
            return null;
        }

        public List<CustomUserDTO> GetAllStaff()
        {
            var staffs = (from u in context.CustomUsers
                          join ur in context.UserRoles on u.Id equals ur.UserId
                          join r in context.Roles on ur.RoleId equals r.Id
                          where r.Name == "Staff"
                          join d in context.Departments on u.DepartmentID equals d.DepartmentID
                          select new CustomUserDTO
                          {
                              Id = u.Id,
                              UserName = u.UserName,
                              Email = u.Email,
                              Address = u.Address,
                              PhoneNumber = u.PhoneNumber,
                              FullName = u.FullName,
                              FileName = u.FileName,
                              Department = context.Departments.FirstOrDefault(s => s.DepartmentID == d.DepartmentID),
                          }
         ).ToList();
            if(staffs != null)
            {
                return staffs;

            }
            return null;
        }

        public CustomUserDTO GetEditCoor(string id)
        {
            var coor = context.CustomUsers.Where(s => s.Id == id).
            Select(u => new CustomUserDTO
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                FullName = u.FullName
            }).FirstOrDefault();
            if (coor != null)
            {
                return coor;
            }
            return null;

        }

        public CustomUserDTO GetEditManager(string id)
        {
            var manager = context.CustomUsers.Where(s => s.Id == id).
           Select(u => new CustomUserDTO
           {
               Id = u.Id,
               UserName = u.UserName,
               Email = u.Email,
               PhoneNumber = u.PhoneNumber,
               FullName = u.FullName
           }).FirstOrDefault();
            if (manager != null)
            {
                return manager;
            }
            return null;

        }

        public CustomUserDTO GetEditStaff(string id)
        {
            var staff = context.CustomUsers.Where(s => s.Id == id).
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
            if(staff != null)
            {
                return staff;
            }
            return null;

        }

        public async Task<CustomUserDTO> PostCreateCoor(CustomUserDTO coor, List<IFormFile> postedFile)
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
            var result = await userManager.CreateAsync(user, "Coor123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Coordinator");
            }
            if(coor != null)
            {
                return coor;
            }
            return null;

        }

        public async Task<CustomUserDTO> PostCreateManager(CustomUserDTO manager, List<IFormFile> postedFile)
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
            var result = await userManager.CreateAsync(user, "Manager123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Assurance");
            }
            if(manager != null)
            {
                return manager;
            }
            return null;
        }

        //public async Task<CustomUserDTO> PostCreateStaff(CustomUserDTO staff, List<IFormFile> postedFile)
        //{
        //    foreach (IFormFile f in postedFile)
        //    {
        //        using (var dataStream = new MemoryStream())
        //        {
        //            await f.CopyToAsync(dataStream);
        //            staff.Image = dataStream.ToArray();
        //        }
        //        if (postedFile != null)
        //        {
        //            //chi dinh duong dan se luu
        //            string fullPath = Path.Combine(Directory.GetCurrentDirectory(),
        //                "wwwroot", "Img", f.FileName);
        //            staff.FileName = f.FileName;
        //            using (var file = new FileStream(fullPath, FileMode.Create))
        //            {
        //                f.CopyTo(file);
        //            }
        //        }
        //    }
        //    var user = new CustomUser
        //    {
        //        UserName = staff.UserName,
        //        FullName = staff.FullName,
        //        Address = staff.Address,
        //        Email = staff.Email,
        //        PhoneNumber = staff.PhoneNumber,
        //        Image = staff.Image,
        //        FileName = staff.FileName,
        //        DepartmentID = staff.DepartmentID
        //    };
        //    var result = await userManager.CreateAsync(user, "Staff123!");
        //    if (result.Succeeded)
        //    {
        //        await userManager.AddToRoleAsync(user, "Staff");
        //    }
        //    if(staff!= null)
        //    {
        //        return staff;
        //    }
        //    return null;

        //}

        public async Task<CustomUserDTO> PostEditManager(CustomUserDTO manager, List<IFormFile> postedFile)
        {
            var newManager = context.CustomUsers.Find(manager.Id);
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
            if(manager == null)
            {
                return null;
            }    
            else
            {
                newManager.UserName = manager.UserName;
                newManager.Email = manager.Email;
                newManager.PhoneNumber = manager.PhoneNumber;
                newManager.FullName = manager.FullName;
                newManager.FileName = manager.FileName;

                context.SaveChanges();
            }
            if (manager != null)
            {
                return manager;
            }
            return null;
          
        }

        public async Task<CustomUserDTO> PostEditStaff(CustomUserDTO staff, List<IFormFile> postedFile)
        {
            var newstaff = context.CustomUsers.Find(staff.Id);
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
            if (postedFile.Count == 0)
            {

                staff.FileName = newstaff.FileName;

            }
            if (newstaff == null)
            {
                return null;
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
                context.SaveChanges();
            }
            return staff;
        }

        public async Task<CustomUserDTO> PostEditCoor(CustomUserDTO coor, List<IFormFile> postedFile)
        {
            var newcoor = context.CustomUsers.Find(coor.Id);
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
                return null;
            }
            else
            {
                newcoor.UserName = coor.UserName;
                newcoor.Email = coor.Email;
                newcoor.PhoneNumber = coor.PhoneNumber;
                newcoor.FullName = coor.FullName;
                context.SaveChanges();
            }
            return coor;
        }

       public bool DeleteCoor(string id)
        {
            var coor = context.Users.FirstOrDefault(u => u.Id == id);
            if (coor == null)
            {
                return false;
            }
            context.Remove(coor);
            context.SaveChanges();
            return true;
        }

        public bool DeleteStaff(string id)
        {
            var staffs = context.CustomUsers.FirstOrDefault(u => u.Id == id);
            var ideas = context.Ideas.Where(i => i.IdeaUserID == id).ToList();
            var comments = context.Comments.Where(c => c.CommentUserID == id).ToList();
            var likes = context.Likes.Where(l => l.LikeUserID == id).ToList();
            if (staffs == null)
            {
                return false;
            }
            context.CustomUsers.Remove(staffs);
            foreach (var i in ideas)
            {
                context.Ideas.Remove(i);
            }
            foreach (var c in comments)
            {
                context.Comments.Remove(c);
            }
            foreach (var l in likes)
            {
                context.Likes.Remove(l);
            }
            context.SaveChanges();
            return true;
        }



        public async Task<CustomUserDTO> PostEditStaff(CustomUserDTO staff, IFormFile postedFile)
        {
   
                using (var dataStream = new MemoryStream())
                {
                    await postedFile.CopyToAsync(dataStream);
                    staff.Image = dataStream.ToArray();
                }
                if (postedFile != null)
                {
                    //chi dinh duong dan se luu
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot", "Img", postedFile.FileName);
                    staff.FileName = postedFile.FileName;
                    using (var file = new FileStream(fullPath, FileMode.Create))
                    {
                        postedFile.CopyTo(file);
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
            var result = await userManager.CreateAsync(user, "Staff123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Staff");
            }
            if (staff != null)
            {
                return staff;
            }
            return null;
        }

        public async Task<CustomUserDTO> PostCreateStaff(CustomUserDTO staff, IFormFile postedFile)
        {
   
            
                using (var dataStream = new MemoryStream())
                {
                    await postedFile.CopyToAsync(dataStream);
                    staff.Image = dataStream.ToArray();
                }
                if (postedFile != null)
                {
                    //chi dinh duong dan se luu
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot", "Img", postedFile.FileName);
                    staff.FileName = postedFile.FileName;
                    using (var file = new FileStream(fullPath, FileMode.Create))
                    {
                        postedFile.CopyTo(file);
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
            var result = await userManager.CreateAsync(user, "Staff123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Staff");
            }
            if (staff != null)
            {
                return staff;
            }
            return null;
        }

        public CustomUserDTO PostEditStaff(CustomUserDTO staff)
        {
            var newstaff = context.CustomUsers.Find(staff.Id);         
            if (newstaff == null)
            {
                return null;
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
                context.SaveChanges();
            }
            return staff;
        }

        public async Task<CustomUserDTO> PostCreateStaff(CustomUserDTO staff)
        {
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
            var result = await userManager.CreateAsync(user, "Staff123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Staff");
            }
            if (staff != null)
            {
                return staff;
            }
            return null;
        }

        public async Task<CustomUserDTO> PostCreateManager(CustomUserDTO manager)
        {
            var user = new CustomUser
            {
                UserName = manager.UserName,
                FullName = manager.FullName,
                Email = manager.Email,
                PhoneNumber = manager.PhoneNumber,
                Image = manager.Image,
                FileName = manager.FileName
            };
            var result = await userManager.CreateAsync(user, "Manager123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Assurance");
            }
            if (manager != null)
            {
                return manager;
            }
            return null;
        }

        public async Task<CustomUserDTO> PostCreateCoor(CustomUserDTO coor)
        {
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
            var result = await userManager.CreateAsync(user, "Coor123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Coordinator");
            }
            if (coor != null)
            {
                return coor;
            }
            return null;

        }

        public CustomUserDTO PostEditCoor(CustomUserDTO coor)
        {
            var newcoor = context.CustomUsers.Find(coor.Id);
            if (newcoor == null)
            {
                return null;
            }
            else
            {
                newcoor.UserName = coor.UserName;
                newcoor.Email = coor.Email;
                newcoor.PhoneNumber = coor.PhoneNumber;
                newcoor.FullName = coor.FullName;
                context.SaveChanges();
            }
            return coor;
        }

        public CustomUserDTO PostEditManager(CustomUserDTO manager)
        {
            var newManager = context.CustomUsers.Find(manager.Id);
           
            if (manager == null)
            {
                return null;
            }
            else
            {
                newManager.UserName = manager.UserName;
                newManager.Email = manager.Email;
                newManager.PhoneNumber = manager.PhoneNumber;
                newManager.FullName = manager.FullName;
                newManager.FileName = manager.FileName;

                context.SaveChanges();
            }
            if (manager != null)
            {
                return manager;
            }
            return null;

        }
    }
}
