using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FTMS.Models
{
	public class TrainerInfo
	{
		[Key]
		public int TrainerInfoId { get; set; }
		[Required]
		public string TrainerId { get; set; }
		public ApplicationUser Trainer { get; set; }
		public IEnumerable<ApplicationUser> Trainers { get; set; }
		public string Full_Name { get; set; }
		public string Email { get; set; }
		public string Working_Place { get; set; }
		public int Phone { get; set; }
	}
}