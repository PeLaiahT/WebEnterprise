using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebEnterprise.Data;
using WebEnterprise.Models;
using WebEnterprise.Models.DTO;
using WebEnterprise.Respon;

namespace TestProject1
{
    public class Tests
    {
        public List<IFormFile> a = new List<IFormFile>();
        
        private static ApplicationDbContext context;
        private static AdminRepo adminRepo;
        private static LikeRepo likeRepo;
        private static CategoryRepo categoryRepo;
        private static DepartmentRepo departmentRepo;
        private static IdeaRepo ideaRepo;
        private static Mock<UserManager<CustomUser>> userManager;
        private string file;

        [SetUp]
        public void Setup()
        {
         
            departmentRepo = new DepartmentRepo(context);
            userManager = new Mock<UserManager<CustomUser>>(
                new Mock<IUserStore<CustomUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<CustomUser>>().Object,
                new IUserValidator<CustomUser>[0],
                new IPasswordValidator<CustomUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<CustomUser>>>().Object);
            context = new ApplicationDbContext();

            userManager.Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new CustomUser()));
            userManager.Setup(userManager => userManager.IsInRoleAsync(It.IsAny<CustomUser>(), "Assurance"))
                .ReturnsAsync(true);
            userManager.Setup(userManager => userManager.IsInRoleAsync(It.IsAny<CustomUser>(), "Staff"))
                .ReturnsAsync(true);
            userManager.Setup(userManager => userManager.IsInRoleAsync(It.IsAny<CustomUser>(), "Coordinator"))
                .ReturnsAsync(true);
            userManager.Setup(userManager => userManager.CreateAsync(It.IsAny<CustomUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            context = new ApplicationDbContext();
            adminRepo = new AdminRepo(context, userManager.Object);
            likeRepo = new LikeRepo(context);
            categoryRepo = new CategoryRepo(context);
            ideaRepo = new IdeaRepo(context);
        }

        [Test]
        public  async Task AddStaff()
        {
            var account = new CustomUserDTO
            {
                UserName = "TestCase1",
                FullName = "Test Case 1",
                Email = "testcase1@gmail.com",
                DepartmentID = 1
            };
            var result = await adminRepo.PostCreateStaff(account);

            Assert.IsTrue(result != null);
        }
        [Test]
        public void TestEditStaff()
        {
            var result = new CustomUserDTO();
            using (var trans = context.Database.BeginTransaction())
            {
                var user = new CustomUserDTO
                {
                    Id = "1",
                    UserName = "Student6",
                    FullName = "Student 7",
                    Email = "student6@gmail.com",
                    DepartmentID = 1,
                };
                result = adminRepo.PostEditStaff(user);

                trans.Rollback();
            }

            Assert.IsTrue(result != null);
        }



        public async Task AddCoor()
        {
            var account = new CustomUserDTO
            {
                UserName = "TestCase1",
                FullName = "Test Case 1",
                Email = "testcase1@gmail.com",
                DepartmentID = 1
            };
            var result = await adminRepo.PostCreateCoor(account);

            Assert.IsTrue(result != null);
        }
        [Test]
        public void TestEditCoor()
        {
            var result = new CustomUserDTO();
            using (var trans = context.Database.BeginTransaction())
            {
                var user = new CustomUserDTO
                {
                    Id = "1",
                    UserName = "Student6",
                    FullName = "Student 7",
                    Email = "student6@gmail.com",
                    DepartmentID = 1,
                };
                result = adminRepo.PostEditCoor(user);

                trans.Rollback();
            }

            Assert.IsTrue(result != null);
        }


        [Test]
        public async Task TestAddCoorFail()
        {
            var account = new CustomUserDTO
            {
                UserName = "TestCase1",
                FullName = "Test Case 1",
            };

            var result = await adminRepo.PostCreateCoor(account);

            Assert.IsFalse(result != null);
        }
        [Test]
        public async Task TestAddStaffFail()
        {
            var account = new CustomUserDTO
            {
                UserName = "TestCase1",
                FullName = "Test Case 1",
            };

            var result = await adminRepo.PostCreateStaff(account);

            Assert.IsFalse(result != null);
        }
        [Test]
        public async Task TestAddManagerFail()
        {
            var account = new CustomUserDTO
            {
                UserName = "TestCase1",
                FullName = "Test Case 1",
            };

            var result = await adminRepo.PostCreateManager(account);

            Assert.IsFalse(result != null);
        }

        [Test]
        public void TestEditManager()
        {
            var result = new CustomUserDTO();
            using (var trans = context.Database.BeginTransaction())
            {
                var user = new CustomUserDTO
                {
                    Id = "1",
                    UserName = "Student6",
                    FullName = "Student 7",
                    Email = "student6@gmail.com",
                    DepartmentID = 1,
                };
                result = adminRepo.PostEditManager(user);

                trans.Rollback();
            }

            Assert.IsTrue(result != null);
        }


        [Test]
        public void GetAdmin()
        {
            var admin = adminRepo.GetAllAdmin();
            Assert.IsTrue(admin != null);
        }

        [Test]
        public void GetAdminfail()
        {
            var admin = adminRepo.GetAllAdmin();
            Assert.IsFalse(admin == null);
        }
        [Test]
        public void GetAllStaffTrue()
        {
            var staffs = adminRepo.GetAllStaff();
            Assert.IsTrue(staffs != null);
        }
        [Test]
        public void DeleteStaff()
        {
            var staff = adminRepo.DeleteStaff("09924af4-b31b-40d7-a119-4201c62c625b");
            Assert.IsTrue(staff != null);

        }
        public void DeleteStaffFail()
        {
            var staff = adminRepo.DeleteStaff("2");
            Assert.IsFalse(staff != null);

        }
        [Test]
        public void DeleteManager()
        {
            var man = adminRepo.DeleteManager("b49e35dd-7a8f-415b-b9c0-9f69179b8504");
            Assert.IsTrue(man != null);

        }

        [Test]
        public void TestDeleteManagerFail()
        {
            bool result;
            using (var trans = context.Database.BeginTransaction())
            {
                string id = "1234";
                result = adminRepo.DeleteManager(id);

                trans.Rollback();
            }

            Assert.IsFalse(result);
        }
        [Test]
        public void TestDeleteStaffFail()
        {
            bool result;
            using (var trans = context.Database.BeginTransaction())
            {
                string id = "1234";
                result = adminRepo.DeleteStaff(id);

                trans.Rollback();
            }

            Assert.IsFalse(result);
        }

        [Test]
        public void TestDeleteCoorFail()
        {
            bool result;
            using (var trans = context.Database.BeginTransaction())
            {
                string id = "1234";
                result = adminRepo.DeleteCoor(id);

                trans.Rollback();
            }

            Assert.IsFalse(result);
        }
        [Test]
        public void DeleteManfFail()
        {
            var man = adminRepo.DeleteManager("2");
            Assert.IsFalse(man != null);

        }
        [Test]
        public void DeleteCoor()
        {
            var coor = adminRepo.DeleteCoor("09924af4-b31b-40d7-a119-4201c62c625b");
            Assert.IsTrue(coor != null);

        }
        [Test]
        public void DeleteCoorfFail()
        {
            var coor = adminRepo.DeleteCoor("2");
            Assert.IsFalse(coor != null);

        }
        [Test]
        public void GetAllStaffFail()
        {
            var staffs = adminRepo.GetAllStaff();
            Assert.IsFalse(staffs == null);
        }
        [Test]
        public void EditStaff()
        {
            string id = "1";
            var staff = adminRepo.GetEditStaff(id);
            Assert.IsTrue(staff.Id == "1"); 
        }
        [Test]
        public void TestEditStaffFail()
        {
            var user = new CustomUserDTO
            {
                Id = "1234",
                UserName = "Student6",
                FullName = "Student 7",
                Email = "student6@gmail.com",
                DepartmentID = 1,
            };
            var result = adminRepo.PostEditStaff(user);

            Assert.IsFalse(result != null);
        }

        [Test]
        public void TestEditManagerFail()
        {
            var user = new CustomUserDTO
            {
                Id = "1234",
                UserName = "Student6",
                FullName = "Student 7",
                Email = "student6@gmail.com",
                DepartmentID = 1,
            };
            var result = adminRepo.PostEditManager(user);

            Assert.IsFalse(result != null);
        }

        [Test]
        public void TestEditCoorFail()
        {
            var user = new CustomUserDTO
            {
                Id = "1234",
                UserName = "Student6",
                FullName = "Student 7",
                Email = "student6@gmail.com",
                DepartmentID = 1,
            };
            var result = adminRepo.PostEditCoor(user);

            Assert.IsFalse(result != null);
        }




        #region Category 

        [Test]
        public void GetAllCatTest()
        {
            var categories = context.Categories
                           .OrderBy(c => c.CategoryID)
                           .ToList();
            Assert.IsTrue(categories != null);
        }

        [Test]

        public void DeleteCatTest()
        {
            bool result;
            using (var trans = context.Database.BeginTransaction())
            {
                var category = context.Categories.FirstOrDefault(t => t.CategoryID == 3);

                if (category != null)
                {
                    context.Categories.Remove(category);
                    context.SaveChanges();
                    result = true;
                }
                else
                {
                    result = false;
                }
                Assert.IsTrue(result);
            }
        }
        [Test]
        public void CreateCategory()
        {
            Category category = new Category
            {
                NameCategory = "Hello",
                Desciption = "This Des",
              
            };
            var cate = categoryRepo.PostCreate(category);
            Assert.IsTrue(cate !=null);
        }
        [Test]
        public void GetEditCategory()
        {
            var cate = categoryRepo.GetUpdate(1);
            Assert.IsTrue(cate.CategoryID == 1);
        }
        [Test]
        public void PostEditCategory()
        {
            var cate = categoryRepo.GetUpdate(1);
            cate.NameCategory = "Hello";
            cate = categoryRepo.PostUpdate(cate);
            Assert.IsTrue(cate.NameCategory =="Hello");
        }

        #endregion


        #region Department



        [Test]
        public void AddDepartTest()
        {
            var result = new Department();
            using (var trans = context.Database.BeginTransaction())
            {
                var depart = new Department
                {
                    NameDepartment = "TestCat1",
                    Description = "TestDes1"
                };
                result = departmentRepo.PostCreate(depart);
                trans.Rollback();
            }
            Assert.IsTrue(result!=null);
        }
        [Test]
        public void AddDepartTesFail()
        {
            var result = new Department();
            using (var trans = context.Database.BeginTransaction())
            {
                result = new Department
                {
                    Description = "TestDes1",
                    NameDepartment = "1",
                };
             result = departmentRepo.PostCreate(result);

                trans.Rollback();
            }
            Assert.IsFalse(result!=null);
        }

        [Test]
        public void AllDepartmentsTest()
        {
            var departments = context.Departments
                            .OrderBy(c => c.DepartmentID)
                            .ToList();
            Assert.IsTrue(departments != null);
        }
        [Test]
        public void EditDepartTest()
        {
            var result = new Department();
            using (var trans = context.Database.BeginTransaction())
            {
                result = new Department
                {
                   
                    NameDepartment = "IT",
                    Description = "Name"
                };
                result = departmentRepo.PostCreate(result);

                trans.Rollback();
            }
            Assert.IsTrue(result!=null);
        }

        [Test]
        public void EditDepartTestFail()
        {
            var result = new Department();
            using (var trans = context.Database.BeginTransaction())
            {
                var test = new Department
                {
                    DepartmentID = 0,
                    NameDepartment = "IT",
                    Description = "Coding"
                };
                result = departmentRepo.PostCreate(test);

                trans.Rollback();
            }
            Assert.IsFalse(result!=null);
        }

        [Test]
        public void DeleteDepartTest()
        {
            bool result;
            using (var trans = context.Database.BeginTransaction())
            {
                var department = context.Departments.Include(d=>d.CustomUsers).FirstOrDefault(t => t.DepartmentID == 2);
               
                if (department != null)
                {         
                    context.Departments.Remove(department);               
                    context.SaveChanges();
                    result = true;
                }
               else
                {
                    result = false;
                }    
          
                trans.Rollback();
            }

            Assert.IsTrue(result);
        }

        [Test]
        public void DeleteDepartTestFail()
        {
            var department = context.Departments.Include(d => d.CustomUsers).FirstOrDefault(t => t.DepartmentID == 0);
            bool result;
            if (department != null)
            {
                context.Departments.Remove(department);
                context.SaveChanges();
                result = true;
            }
            else
            {
                result = false;
            }
            Assert.IsFalse(result);
        }

        #endregion
        #region Like
        [Test]
        public void LikeTest()
        {

            Idea idea = new Idea();
           using (var trans = context.Database.BeginTransaction())
            {
                 idea = new Idea
                {
                    IdeaID = 10,
                    Likecount = 0,
                };
                var like = new Like
                {
                    LikeUserID = "1",
                    LikeId = 1,
                    IdeaId = idea.IdeaID,

                };     
                context.Likes.Add(like);
                idea.Likecount++;
                trans.Rollback();
            }
            Assert.IsTrue(idea.Likecount!=0);
        }

        [Test]
        public void DisLikeTest()
        {
            Idea idea = new Idea();
            using (var trans = context.Database.BeginTransaction())
            {
                idea = new Idea
                {
                    IdeaID = 10,
                    Likecount = 0,
                };
                var like = new Like
                {
                    LikeUserID = "1",
                    LikeId = 1,
                    IdeaId = idea.IdeaID,

                };
                context.Likes.Add(like);
                idea.Likecount++;
                trans.Rollback();
            }
            Assert.IsTrue(idea.Likecount != 0);
        }

        #endregion

        #region Idea
        [Test]
        public void IndexIdea()
        {
            var listIdea = context.Ideas.Include(i => i.Category)
               .Include(i => i.IdeaUser)
               .Include(i => i.Documments)
               .OrderByDescending(i => i.CreateAt);
            Assert.IsTrue(listIdea != null);
        }
        [Test]
        public void GetClosureDate()
        {
            var idea = ideaRepo.GetClosureDate(4);
            Assert.IsTrue(idea != null);
        }
        [Test]
        public void GetClosureDateFail()
        {
            var idea = ideaRepo.GetClosureDate(1000);
            Assert.IsFalse(idea != null);
        }
        [Test]
        public void PostClosureDate()
        {
            var idea = ideaRepo.GetClosureDate(4);
            idea.FirstDate = DateTime.Today;
             idea = ideaRepo.PostClosureDate(idea);
            Assert.IsTrue(idea.FirstDate == DateTime.Today);
        }
        [Test]
        public void DeleteIdea()
        {
            int id = 4;
            var idea = context.Ideas.FirstOrDefault(t => t.IdeaID == id);
            if (idea != null)
            {
                context.Ideas.Remove(idea);
                context.SaveChanges();
            }
            Assert.IsTrue(idea != null);
        }

        [Test]
        public void DeleteIdeaFalse()
        {
            int id = 4;
            var idea = context.Ideas.FirstOrDefault(t => t.IdeaID == id);
            if (idea != null)
            {
                context.Ideas.Remove(idea);
                context.SaveChanges();
            }
            Assert.IsTrue(idea != null);
        }



        [Test]
        public void AddIdea()
        {
            var result = new Idea();
            using (var trans = context.Database.BeginTransaction())
            {
                var test = new Idea
                {
                    Title = "Test123",
                    Content = "Test123",
                    CategoryID = 1,
                };
                result = ideaRepo.PostCreate(test);

                trans.Rollback();
            }
            Assert.IsTrue(result!=null);
        }

        [Test]
        public void CreateIdeaFail()
        {
            var result = new Idea();
            using (var trans = context.Database.BeginTransaction())
            {
                var test = new Idea
                {
                    Title = "Test123",
                    CategoryID = 1,
                };
                result = ideaRepo.PostCreate(test);

                trans.Rollback();
            }
            Assert.IsTrue(result!=null);
        }


        [Test]
        public void EditIdeaTest()
        {
            var result = new Idea();
            using (var trans = context.Database.BeginTransaction())
            {
                var test = new Idea
                {
                    IdeaID = 8,
                    Title = "Test123",
                   Content = "Test123",
                    FirstDate = DateTime.Now,
                    LastDate = DateTime.Now.AddDays(3),
                    CategoryID = 1,
                };
                result = ideaRepo.PostUpdate(test);

                trans.Rollback();
            }
            Assert.IsTrue(result!=null);
        }

        [Test]
        public void EditIdeaTestFail()
        {
            var result = new Idea();
            using (var trans = context.Database.BeginTransaction())
            {
                var test = new Idea
                {
                    IdeaID = 8,
                    Title = "Test123",
                    Content = "Test123",
                    FirstDate = DateTime.Now,
                    LastDate = DateTime.Now.AddDays(3),
                    CategoryID = 1,
                };
                result = ideaRepo.PostUpdate(test);

                trans.Rollback();
            }
            Assert.IsFalse(result!=null);
        }

        [Test]
        public void DeleteDoc()
        {
            bool result;
            using (var trans = context.Database.BeginTransaction())
            {
                int id = 1;
                result = ideaRepo.DeleteDoc(id);
                trans.Rollback();
            }
            Assert.IsTrue(result);
        }

        [Test]
        public void DeleteDocFalse()
        {
            bool result;
            using (var trans = context.Database.BeginTransaction())
            {
                int id = 1000;
                result = ideaRepo.DeleteDoc(id);
                trans.Rollback();
            }
            Assert.IsFalse(result);
        }
        #endregion
        #region Comment
        [Test]
        public void CreateComment()
        {
            var idea = ideaRepo.GetUpdate(8);
            
            var comment = new Comment
            {
                Content = "Hello",
                IdeaID = 8,
                
            };
            context.Comments.Add(comment);
            context.SaveChanges();
            Assert.IsTrue(idea.Comments.Count != 0);
               
            
        }
        [Test]
        public void CreateCommentFalse()
        {
            var idea = ideaRepo.GetUpdate(8);
            

            var comment = new Comment
            {
                Content = "Hello",
                IdeaID = 8,

            };
            context.Comments.Add(comment);
            context.SaveChanges();
            Assert.IsFalse(idea.Comments.Count != 0);


        }
        #endregion


    }
}