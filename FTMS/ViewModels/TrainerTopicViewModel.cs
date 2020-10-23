using FTMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FTMS.ViewModels
{
	public class TrainerTopicViewModel
	{
		public ManageTrainer ManageTrainer { get; set; }
		public Topic Topic { get; set; }
		public IEnumerable<Topic> Topics { get; set; }
		public IEnumerable<ApplicationUser> Trainers { get; set; }
	}
}