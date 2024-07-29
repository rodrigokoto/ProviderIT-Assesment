﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Core.Models.BaseModel;

namespace Core.Models
{
    public class User : IdentityBase
	{
	

		[Required]
		public string UserName { get; set; }

		public string? Email { get; set; }

		public string? PasswordHash { get; set; }

		public string? SecurityStamp { get; set; }

		public string? ConcurrencyStamp { get; set; }

		public string? PhoneNumber { get; set; }

		public bool EmailConfirmed { get; set; }

		public bool PhoneNumberConfirmed { get; set; }

		public bool TwoFactorEnabled { get; set; }

		public DateTime? LockoutEnd { get; set; }

		public bool LockoutEnabled { get; set; }

		public int AccessFailedCount { get; set; }

		public ICollection<UserRole> UserRoles { get; set; }
	}
}
