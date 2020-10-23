using FTMS.Models;
using FTMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace FTMS.Controllers
{
	public class ManageTraineesController : Controller
	{
		private ApplicationDbContext _context;
		public ManageTraineesController()
		{
			_context = new ApplicationDbContext();
		}

		[Authorize(Roles = "Training Staff")]
		public ActionResult Index()
		{
			var managetrainees = _context.ManageTrainees.Include(m => m.Course)
														.Include(m => m.Trainee)
														.ToList();

			return View(managetrainees);
		}

		[HttpGet]
		[Authorize(Roles = "Training Staff")]
		public ActionResult Create()
		{
			//Get Trainee
			var role = (from r in _context.Roles where r.Name.Contains("Trainee") select r)
															 .FirstOrDefault();
			var users = _context.Users.Where(x => x.Roles.Select(y => y.RoleId)
									  .Contains(role.Id))
								      .ToList();
			//Get Course
			var courses = _context.Courses.ToList();

			var newTraineeTopic = new TraineeCourseViewModel
			{
				Courses = courses,
				Trainees = users,
				ManageTrainee = new ManageTrainee()
			};

			return View(newTraineeTopic);
		}

		[HttpPost]
		[Authorize(Roles = "Training Staff")]
		public ActionResult Create(ManageTrainee manageTrainee)
		{
			var newTrainee = new ManageTrainee
			{
				TraineeId = manageTrainee.TraineeId,
				CourseId = manageTrainee.CourseId
			};

			_context.ManageTrainees.Add(newTrainee);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}


		[HttpGet]
		[Authorize(Roles = "Trainee")]
		public ActionResult Mine()
		{
			var userId = User.Identity.GetUserId();

			var manageTrainees = _context.ManageTrainees
				.Where(c => c.TraineeId == userId)
				.Include(c => c.Course)
				.Include(c => c.Trainee)
				.ToList();

			return View(manageTrainees);
		}



		[HttpGet]
		[Authorize(Roles = "Training Staff")]
		public ActionResult Delete(int id)
		{
			var traineeCourseInDb = _context.ManageTrainees.SingleOrDefault(c => c.Id == id);
			if (traineeCourseInDb == null)
			{
				return HttpNotFound();
			}
			_context.ManageTrainees.Remove(traineeCourseInDb);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}