using FTMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace FTMS.Controllers
{
	public class TraineeInfosController : Controller
	{
		private ApplicationDbContext _context;

		public TraineeInfosController()
		{
			_context = new ApplicationDbContext();
		}

		[Authorize(Roles = "Training Staff")]
		public ActionResult Index(string searchString)
		{
			var traineeInfos = _context.TraineeInfos.Include(u => u.Trainee)
													.ToList();
			if (!String.IsNullOrEmpty(searchString))
			{
				traineeInfos = traineeInfos.FindAll(s => s.Trainee.UserName.Contains(searchString));
			}
			return View(traineeInfos);
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

			var newTrainee = new TraineeInfo
			{
				Trainees = users,
			};
			return View(newTrainee);
		}

		[HttpPost]
		[Authorize(Roles = "Training Staff")]
		public ActionResult Create(TraineeInfo traineeInfo)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			/*if (_context.TraineeInfos.Any(c => c.Full_Name.Contains(traineeInfo.Full_Name)))
			{
				ModelState.AddModelError("Name", "Trainer Name Already Exists.");
				return View();
			}*/
			var newTraineeInfo = new TraineeInfo
			{
				TraineeId = traineeInfo.TraineeId,
				Full_Name = traineeInfo.Full_Name,
				Education = traineeInfo.Education,
				Programming_Language = traineeInfo.Programming_Language,
				Age = traineeInfo.Age,
				Experience_Details = traineeInfo.Experience_Details,
				Location = traineeInfo.Location,
				Toeic_Score = traineeInfo.Toeic_Score,
				Department = traineeInfo.Department,
			};
			_context.TraineeInfos.Add(newTraineeInfo);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}

		[HttpGet]
		[Authorize(Roles = "Trainee")]
		public ActionResult Mine()
		{
			var userId = User.Identity.GetUserId();

			var traineesDetail = _context.TraineeInfos
				.Where(c => c.TraineeId == userId)
				.Include(c => c.Trainee)
				.ToList();

			return View(traineesDetail);
		}


		[HttpGet]
		[Authorize(Roles = "Training Staff")]
		public ActionResult Delete(int traineeInfoId)
		{
			var traineeInfoInDb = _context.TraineeInfos.SingleOrDefault(p => p.TraineeInfoId == traineeInfoId);
			if (traineeInfoInDb == null)
			{
				return HttpNotFound();
			}
			_context.TraineeInfos.Remove(traineeInfoInDb);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}

		[HttpGet]
		[Authorize(Roles = "Training Staff, Trainee")]
		public ActionResult Edit(int traineeInfoId)
		{
			var traineeInfoInDb = _context.TraineeInfos.SingleOrDefault(p => p.TraineeInfoId == traineeInfoId);

			if (traineeInfoInDb == null)
			{
				return HttpNotFound();
			}

			return View(traineeInfoInDb);
		}

		[HttpPost]
		[Authorize(Roles = "Training Staff,Trainee")]
		public ActionResult Edit(TraineeInfo traineeInfo)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			var traineeInfoInDb = _context.TraineeInfos.SingleOrDefault(c => c.TraineeInfoId == traineeInfo.TraineeInfoId);

			if (traineeInfoInDb == null)
			{
				return HttpNotFound();
			}

			traineeInfoInDb.TraineeId = traineeInfo.TraineeId;
			traineeInfoInDb.Full_Name = traineeInfo.Full_Name;
			traineeInfoInDb.Education = traineeInfo.Education;
			traineeInfoInDb.Programming_Language = traineeInfo.Programming_Language;
			traineeInfoInDb.Age = traineeInfo.Age;
			traineeInfoInDb.Experience_Details = traineeInfo.Experience_Details;
			traineeInfoInDb.Location = traineeInfo.Location;
			traineeInfoInDb.Toeic_Score = traineeInfo.Toeic_Score;
			traineeInfoInDb.Department = traineeInfo.Department;
			_context.SaveChanges();
			if (User.IsInRole("Training Staff"))
			{
				return RedirectToAction("Index");
			}
			if (User.IsInRole("Trainee"))
			{
				return RedirectToAction("Mine");
			}
			return RedirectToAction("Index");
		}
	}
}