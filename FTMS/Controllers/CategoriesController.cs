using FTMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FTMS.Controllers
{
	public class CategoriesController : Controller
	{
		private ApplicationDbContext _context;

		public CategoriesController()
		{
			_context = new ApplicationDbContext();
		}
		// GET: Categories
		[Authorize(Roles = "Training Staff")]
		public ActionResult Index(string searchString)
		{
			var categories = _context.Categories.ToList();
			if (!String.IsNullOrEmpty(searchString))
			{
				categories = categories.FindAll(s => s.Name.Contains(searchString));
			}
			return View(categories);
		}

		// Create: Categories
		[HttpGet]
		[Authorize(Roles = "Training Staff")]
		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[Authorize(Roles = "Training Staff")]
		public ActionResult Create(Category category)
		{
			if (!ModelState.IsValid)
			{
				return View("~/Views/ErrorValidations/Null.cshtml");
			}

			var checkCategory = _context.Categories.Any(c => c.Name == category.Name);
			if (checkCategory == true)
			{
				return View("~/Views/ErrorValidations/Exist.cshtml");
			}

			var newCategory = new Category
			{
				Name = category.Name,
				Description = category.Description
			};
			_context.Categories.Add(newCategory);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}

		[HttpGet]
		[Authorize(Roles = "Training Staff")]
		public ActionResult Delete(int id)
		{
			var categoryInDb = _context.Categories.SingleOrDefault(c => c.Id == id);
			if (categoryInDb == null)
			{
				return HttpNotFound();
			}
			_context.Categories.Remove(categoryInDb);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}

		[HttpGet]
		[Authorize(Roles = "Training Staff")]
		public ActionResult Edit(int id)
		{
			var categoryInDb = _context.Categories.SingleOrDefault(c => c.Id == id);
			if (categoryInDb == null)
			{
				return HttpNotFound();
			}
			return View(categoryInDb);
		}
		[HttpPost]
		[Authorize(Roles = "Training Staff")]
		public ActionResult Edit(Category category)
		{
			if (!ModelState.IsValid)
			{
				return View("~/Views/ErrorValidations/Null.cshtml");
			}

			var checkCategory = _context.Categories.Any(c => c.Name == category.Name);
			
			if (checkCategory == true)
			{
				return View("~/Views/ErrorValidations/Exist.cshtml");
			}

			var categoryInDb = _context.Categories.SingleOrDefault(c => c.Id == category.Id);
			
			if (categoryInDb == null)
			{
				return HttpNotFound();
			}

			categoryInDb.Name = category.Name;
			categoryInDb.Description = category.Description;
			_context.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}