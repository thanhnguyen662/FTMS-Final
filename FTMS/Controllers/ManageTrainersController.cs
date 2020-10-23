﻿using FTMS.Models;
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
	public class ManageTrainersController : Controller
	{
		private ApplicationDbContext _context;
		public ManageTrainersController()
		{
			_context = new ApplicationDbContext();
		}

		[Authorize(Roles = "Training Staff")]
		public ActionResult Index()
		{
			var managetrainers = _context.ManageTrainers.Include(m => m.Topic)
														.Include(m => m.Trainer)
														.ToList();
			return View(managetrainers);
		}

		[HttpGet]
		[Authorize(Roles = "Training Staff")]
		public ActionResult Create()
		{
			{
				//Get trainer
				var role = (from r in _context.Roles where r.Name.Contains("Trainer") select r)
																 .FirstOrDefault();
				var users = _context.Users.Where(x => x.Roles.Select(y => y.RoleId)
										  .Contains(role.Id))
										  .ToList();
				//Get Topic
				var topics = _context.Topics.ToList();

				var newTrainerTopic = new TrainerTopicViewModel
				{
					Topics = topics,
					Trainers = users,
					ManageTrainer = new ManageTrainer()
				};

				return View(newTrainerTopic);
			}
		}

		[HttpPost]
		[Authorize(Roles = "Training Staff")]
		public ActionResult Create(ManageTrainer manageTrainer)
		{
			var newTrainer = new ManageTrainer
			{
				TrainerId = manageTrainer.TrainerId,
				TopicId = manageTrainer.TopicId
			};

			_context.ManageTrainers.Add(newTrainer);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}

		[HttpGet]
		[Authorize(Roles = "Trainer")]
		public ActionResult Mine()
		{
			var userId = User.Identity.GetUserId();

			var manageTrainers = _context.ManageTrainers
				.Where(c => c.TrainerId == userId)
				.Include(c => c.Topic)
				.Include(c => c.Trainer)
				.ToList();

			return View(manageTrainers);
		}


		[HttpGet]
		[Authorize(Roles = "Training Staff")]
		public ActionResult Delete(int id)
		{
			var trainerTopicInDb = _context.ManageTrainers.SingleOrDefault(c => c.Id == id);
			if (trainerTopicInDb == null)
			{
				return HttpNotFound();
			}
			_context.ManageTrainers.Remove(trainerTopicInDb);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}