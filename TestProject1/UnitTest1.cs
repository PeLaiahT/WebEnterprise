using NUnit.Framework;
using System.Linq;
using WebEnterprise.Data;
using WebEnterprise.Respon;

namespace TestProject1
{
    public class Tests
    {
        private static ApplicationDbContext context;
        private static AdminRepo adminRepo;

        [SetUp]
        public void Setup()
        {
            context = new ApplicationDbContext();
            adminRepo = new AdminRepo(context);
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
        //[Test]
        //public void GetEditStaff(string id)
        //{
        //    string id1 = "1";
        //    var staffs = adminRepo.GetEditStaff(id);
        //    Assert.IsTrue(id1);
        //}


    }
}