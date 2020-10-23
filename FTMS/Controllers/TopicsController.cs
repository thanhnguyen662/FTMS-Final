using FTMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using FTMS.ViewModels;

namespace FTMS.Controllers
{
	public class TopicsController : Controller
	{
		private ApplicationDbContext _context;
		public TopicsController()
		{
			_context = new ApplicationDbContext();
		}
		// GET: Topics
		[Authorize(Roles = "Training Staff")]
		public ActionResult Index(string searchString)
		{
			var topics = _context.Topics.Include(p => p.Course);
			if (!String.IsNullOrEmpty(searchString))
			{
				topics = topics.Where(s => s.Name.Contains(searchString) || s.Course.Name.Contains(searchString));
			}
			return View(topics.ToList());
		}
		// Create: Topic
		[HttpGet]
		[Authorize(Roles = "Training Staff")]
		public ActionResult Create()
		{
			var viewModel = new TopicCourseViewModel
			{
				Courses = _context.Courses.ToList()
			};
			return View(viewModel);
		}

		[HttpPost]
		[Authorize(Roles = "Training Staff")]
		public ActionResult Create(Topic topic)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			/*if (_context.Topics.Any(c => c.Name.Contains(topic.Name)))
			{
				ModelState.AddModelError("Name", "Topic Name Already Exists.");
				return View();
			}*/
			var newTopic = new Topic
			{
				Name = topic.Name,
				Description = topic.Description,
				CourseId = topic.CourseId
			};
			_context.Topics.Add(newTopic);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}

		[HttpGet]
		[Authorize(Roles = "Training Staff")]
		public ActionResult Delete(int id)
		{
			var topicInDb = _context.Topics.SingleOrDefault(c => c.Id == id);
			if (topicInDb == null)
			{
				return HttpNotFound();
			}
			_context.Topics.Remove(topicInDb);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}

		[HttpGet]
		[Authorize(Roles = "Training Staff")]
		public ActionResult Edit(int id)
		{
			var topicInDb = _context.Topics.SingleOrDefault(c => c.Id == id);
			if (topicInDb == null)
			{
				return HttpNotFound();
			}

			var viewModel = new TopicCourseViewModel
			{
				Topic = topicInDb,
				Courses = _context.Courses.ToList()
			};

			return View(viewModel);
		}
		[HttpPost]
		[Authorize(Roles = "Training Staff")]
		public ActionResult Edit(Topic topic)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			var topicInDb = _context.Topics.SingleOrDefault(c => c.Id == topic.Id);
			if (topicInDb == null)
			{
				return HttpNotFound();
			}
			topicInDb.Name = topic.Name;
			topicInDb.Description = topic.Description;
			topicInDb.CourseId = topic.CourseId;
			_context.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}