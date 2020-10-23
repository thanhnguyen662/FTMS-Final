using FTMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FTMS.ViewModels
{
	public class TraineeCourseViewModel
	{
		public ManageTrainee ManageTrainee { get; set; }
		public Course Course { get; set; }
		public IEnumerable<Course> Courses { get; set; }
		public IEnumerable<ApplicationUser> Trainees { get; set; }
	}
}