#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebEnterprise.Data;
using WebEnterprise.Models;
using WebEnterprise.Respon;

namespace WebEnterprise.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IDepartmentRepo departmentRepo;
        public DepartmentsController(ApplicationDbContext db , IDepartmentRepo _departmentRepo)
        {
            _db = db;
            departmentRepo = _departmentRepo;
        }
        public IActionResult Index()
        {
            
            var departments = departmentRepo.GetListCategory();
            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (!ModelState.IsValid)
            {
                return View(department);
            }
            else
            {
                _db.Departments.Add(department);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

        }

        public IActionResult Delete(int id)
        {
            var department = departmentRepo.DeleteDepartment(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            var department = departmentRepo.GetUpdate(id);
            if (department != null)
            {
                return View(department);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Update(Department department)
        {
            if (ModelState.IsValid)
            {
                return View(department);
            }
            else
            {
                _db.Entry(department).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}
