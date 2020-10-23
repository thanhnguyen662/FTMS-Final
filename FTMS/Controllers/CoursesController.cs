using FTMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using FTMS.ViewModels;
using static FTMS.Controllers.ManageController;

namespace FTMS.Controllers
{
	public class CoursesController : Controller
	{
		private ApplicationDbContext _context;

		public CoursesController()
		{
			_context = new ApplicationDbContext();
		}

		[HttpGet]
		[Authorize(Roles = "Training Staff")]
		public ActionResult Index(string searchString)
		{
			var courses = _context.Courses
			.Include(p => p.Category);

			if (!String.IsNullOrEmpty(searchString))
			{
				courses = courses.Where(
					s => s.Name.Contains(searchString) ||
					s.Category.Name.Contains(searchString));
			}

			return View(courses.ToList());
		}
		// Create: Categories
		[HttpGet]
		[Authorize(Roles = "Training Staff")]
		public ActionResult Create()
		{
			var viewModel = new CourseCategoryViewModel
			{
				Categories = _context.Categories.ToList()
			};
			return View(viewModel);
		}

		[HttpPost]
		[Authorize(Roles = "Training Staff")]
		public ActionResult Create(Course course)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			/*if (_context.Courses.Any(c => c.Name.Contains(course.Name)))
			{
				ModelState.AddModelError("Name", "Course Name Already Exists.");
				return View();
			}*/
				
			var newCourses = new Course
			{
				Name = course.Name,
				Description = course.Description,
				CategoryID = course.CategoryID
			};
			_context.Courses.Add(newCourses);
			_context.SaveChanges();
			return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
		}

		[HttpGet]
		[Authorize(Roles = "Training Staff")]
		public ActionResult Delete(int id)
		{
			var courseInDb = _context.Courses.SingleOrDefault(c => c.Id == id);
			if (courseInDb == null)
			{
				return HttpNotFound();
			}
			_context.Courses.Remove(courseInDb);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}

		[HttpGet]
		[Authorize(Roles = "Training Staff")]
		public ActionResult Edit(int id)
		{
			var courseInDb = _context.Courses.SingleOrDefault(c => c.Id == id);
			if (courseInDb == null)
			{
				return HttpNotFound();
			}
			var viewModel = new CourseCategoryViewModel
			{
				Course = courseInDb,
				Categories = _context.Categories.ToList()
			};
			return View(viewModel);
		}
		[HttpPost]
		[Authorize(Roles = "Training Staff")]
		public ActionResult Edit(Course course)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			var courseInDb = _context.Courses.SingleOrDefault(c => c.Id == course.Id);
			if (courseInDb == null)
			{
				return HttpNotFound();
			}
			courseInDb.Name = course.Name;
			courseInDb.Description = course.Description;
			courseInDb.CategoryID = courseInDb.CategoryID;
			_context.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}