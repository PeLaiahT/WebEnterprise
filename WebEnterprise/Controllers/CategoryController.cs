﻿using Microsoft.AspNetCore.Mvc;
using WebEnterprise.Data;
using WebEnterprise.Models;

namespace WebEnterprise.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var categories = _db.Categories
                             .OrderBy(c => c.IdCategory)
                             .ToList();
            return View(categories);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            else
            {
                _db.Categories.Add(category);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

        }

        public IActionResult Delete(int id)
        {
            var courseCategory = _db.Categories.FirstOrDefault(t => t.IdCategory == id);
            if (courseCategory != null)
            {
                _db.Categories.Remove(courseCategory);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Update( int id)
        {
            var category = _db.Categories.FirstOrDefault(t => t.IdCategory == id);
            if (category != null)
            {
                return View(category);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Update(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            else
            {
                //_db.Entry<Category>(category).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}
