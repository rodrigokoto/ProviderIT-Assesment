using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Core.Models.BaseModel;

namespace Core.Models
{
    public class Tasks : IdentityBase
	{

		[Required]
		public string Title { get; set; }

		public string Description { get; set; }

		public DateTime DueDate { get; set; }

		[ForeignKey("User")]
		public int UserId { get; set; }
		public User User { get; set; }

		public Tasks tasks { get; set; }

		[Required]
		public Core.Enums.ProviderTaskStatus Status { get; set; }

	}
}
