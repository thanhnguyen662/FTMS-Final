using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FTMS.Models
{
	public class Course
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Description { get; set; }

		[Required]
		public int CategoryID { get; set; }
		public Category Category { get; set; }
	}
}