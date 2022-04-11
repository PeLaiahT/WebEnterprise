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
        private static Mock<UserManager<CustomUser>> userManager;
        private string file;

        [SetUp]
        public void Setup()
        {
            context = new ApplicationDbContext();
            adminRepo = new AdminRepo(context);
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
        }

        [Test]
        public async Task AddStaff()
        {
            using var stream = new MemoryStream(File.ReadAllBytes(file).ToArray());
            var formFile = new FormFile(stream, 0, stream.Length, "streamFile", file.Split(@"\").Last());

            var account = new CustomUserDTO
            {
                UserName = "TestCase1",
                FullName = "Test Case 1",
                Email = "testcase1@gmail.com",
                DepartmentID = 1
            };

            var result = await adminRepo.PostCreateStaff(account, formFile);

            Assert.IsTrue(result != null);
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
        public void GetAdmin()
        {
            var admin = adminRepo.GetAllAdmin();
            Assert.IsTrue(admin != null);
        }

        [Test]
        public void GetAdminfail()
        {
            var admin = adminRepo.GetAllAdmin();
            Assert.IsFalse(admin != null);
        }
        [Test]
        public void GetAllStaffTrue()
        {
            var staffs = adminRepo.GetAllStaff();
            Assert.IsTrue(staffs != null);
        }
        [Test]
        public void GetAllStaffFail()
        {
            var staffs = adminRepo.GetAllStaff();
            Assert.IsFalse(staffs != null);
        }
        [Test]
        public void EditStaff()
        {
            string id = "1";
            var staff = adminRepo.GetEditStaff(id);
            Assert.IsTrue(staff.Id == "1");
        }
        [Test]
        public void Edit()
        {
            var result = new CustomUserDTO();
            using (var trans = context.Database.BeginTransaction())
            {
                var user = new CustomUserDTO
                {
                    Id = "60395add-292d-4224-8d54-f9376bba5e66",
                    UserName = "Student6",
                    FullName = "Student 7",
                    Email = "student6@gmail.com",
                    DepartmentID = 1,
                };

                trans.Rollback();
            }


            Assert.IsTrue(result != null);
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
                var department = context.Categories.FirstOrDefault(t => t.CategoryID == 1);

                if (department != null)
                {
                    //context.Departments.Remove(department);
                    context.SaveChanges();
                    result = true;
                }
                else
                {
                    result = false;
                }
                Assert.IsTrue(result);
            }



            #endregion


            #region Department

        }

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
                Assert.IsTrue(result != null);
            }
            [Test]
            public void AddDepartTesFail()
            {
                Department result;
                using (var trans = context.Database.BeginTransaction())
                {
                    var depart = new Department
                    {
                        Description = "TestDes1",
                    };
                    result = departmentRepo.PostCreate(depart);

                    trans.Rollback();
                }
                Assert.IsFalse(result != null);
            }

            [Test]
            public void EditDepartTest()
            {
                var result = new Department();
                using (var trans = context.Database.BeginTransaction())
                {
                    var test = new Department
                    {
                        DepartmentID = 1,
                        NameDepartment = "IT",
                        Description = "Name"
                    };
                    result = departmentRepo.PostCreate(test);

                    trans.Rollback();
                }
                Assert.IsTrue(result != null);
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
                Assert.IsFalse(result != null);
            }

            [Test]
            public void DeleteDepartTest()
            {
                bool result;
                using (var trans = context.Database.BeginTransaction())
                {
                    var department = context.Departments.Include(d => d.CustomUsers).FirstOrDefault(t => t.DepartmentID == 2);

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
            //[Test]
            //public void LikeTest()
            //{
            //    Like result;
            //    Idea a = new Idea();

            //    using (var trans = context.Database.BeginTransaction())
            //    {
            //        var like = new Like
            //        {
            //           LikeUserID  = "1",
            //            LikeId = 1,
            //        };

            //        result = (likeRepo.UpLike(1));
            //        trans.Rollback();
            //    }
            //    Assert.IsTrue(result.Status);
            //}

            //[Test]
            //public void DisLikeTest()
            //{
            //    UserLikePost result;
            //    using (var trans = context.Database.BeginTransaction())
            //    {
            //        var like = new UserLikePost
            //        {
            //            UserId = "1",
            //            PostId = 1,
            //        };

            //        result = userRepo.Dislike(like);
            //        trans.Rollback();
            //    }
            //    Assert.IsTrue(!result.Status);
            //}

            #endregion

        }
    }