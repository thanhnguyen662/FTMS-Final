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
	public class TrainerInfosController : Controller
	{
		private ApplicationDbContext _context;

		public TrainerInfosController()
		{
			_context = new ApplicationDbContext();
		}

		[Authorize(Roles = "Training Staff")]
		public ActionResult Index(string searchString)
		{
			var trainerInfos = _context.TrainerInfos.Include(u => u.Trainer)
													.ToList();

			if (!String.IsNullOrEmpty(searchString))
			{
				trainerInfos = trainerInfos.FindAll(s => s.Trainer.UserName.Contains(searchString));
			}

			return View(trainerInfos);
		}

		[HttpGet]
		[Authorize(Roles = "Training Staff")]
		public ActionResult Create()
		{
			//Get trainer
			var role = (from r in _context.Roles where r.Name.Contains("Trainer") select r)
															 .FirstOrDefault();
			var users = _context.Users.Where(x => x.Roles.Select(y => y.RoleId)
									  .Contains(role.Id))
									  .ToList();
			//Get Topic
			var topics = _context.Topics.ToList();


			var newTrainer = new TrainerInfo
			{
				Trainers = users,
			};
			return View(newTrainer);
		}

		[HttpPost]
		[Authorize(Roles = "Training Staff")]
		public ActionResult Create(TrainerInfo trainerInfo)
		{
			if (!ModelState.IsValid)
			{
				return View("~/Views/ErrorValidations/Null.cshtml");
			}

			var newTrainerInfo = new TrainerInfo
			{
				TrainerId = trainerInfo.TrainerId,
				Full_Name = trainerInfo.Full_Name,
				Email = trainerInfo.Email,
				Working_Place = trainerInfo.Working_Place,
				Phone = trainerInfo.Phone
			};
			_context.TrainerInfos.Add(newTrainerInfo);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}

		[HttpGet]
		[Authorize(Roles = "Trainer")]
		public ActionResult Mine()
		{
			var userId = User.Identity.GetUserId();

			var trainersDetail = _context.TrainerInfos
				.Where(c => c.TrainerId == userId)
				.Include(c => c.Trainer)
				.ToList();

			return View(trainersDetail);
		}


		[HttpGet]
		[Authorize(Roles = "Training Staff")]
		public ActionResult Delete(int trainerInfoId)
		{
			var trainerInfoInDb = _context.TrainerInfos.SingleOrDefault(p => p.TrainerInfoId == trainerInfoId);
			if (trainerInfoInDb == null)
			{
				return HttpNotFound();
			}
			_context.TrainerInfos.Remove(trainerInfoInDb);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult Edit(int trainerInfoId)
		{
			var trainerInfoInDb = _context.TrainerInfos.SingleOrDefault(p => p.TrainerInfoId == trainerInfoId);

			if (trainerInfoInDb == null)
			{
				return HttpNotFound();
			}

			return View(trainerInfoInDb);
		}

		[HttpPost]
		[Authorize(Roles = "Training Staff,Trainer")]
		public ActionResult Edit(TrainerInfo trainerInfo)
		{
			if (!ModelState.IsValid)
			{
				return View("~/Views/ErrorValidations/Null.cshtml");
			}
			var trainerInfoInDb = _context.TrainerInfos.SingleOrDefault(c => c.TrainerInfoId == trainerInfo.TrainerInfoId);

			if (trainerInfoInDb == null)
			{
				return HttpNotFound();
			}

			trainerInfoInDb.TrainerId = trainerInfo.TrainerId;
			trainerInfoInDb.Full_Name = trainerInfo.Full_Name;
			trainerInfoInDb.Email = trainerInfo.Email;
			trainerInfoInDb.Working_Place = trainerInfo.Working_Place;
			_context.SaveChanges();
			if (User.IsInRole("Training Staff"))
			{
				return RedirectToAction("Index");
			}
			if (User.IsInRole("Trainer"))
			{
				return RedirectToAction("Mine");
			}
			return RedirectToAction("Index");
		}
	}
}