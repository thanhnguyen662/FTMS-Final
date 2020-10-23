using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace FTMS.Models
{
	public class TraineeInfo
	{
		[Key]
		public int TraineeInfoId { get; set; }
		[Required]
		public string TraineeId { get; set; }
		public ApplicationUser Trainee { get; set; }
		public IEnumerable<ApplicationUser> Trainees { get; set; }
		public string Full_Name { get; set; }
		public string Education { get; set; }
		public string Programming_Language { get; set; }
		public int Age { get; set; }
		public string Experience_Details { get; set; }
		public string Location { get; set; }
		public int Toeic_Score { get; set; }
		public string Department { get; set; }
	}
}