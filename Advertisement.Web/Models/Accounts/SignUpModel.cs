﻿using System.ComponentModel.DataAnnotations;

namespace Advertisement.Web.Models.Accounts
{
	public class SignUpModel
	{
		[Required]
		[EmailAddress]
		[Display(Name="Email")]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[StringLength(8,ErrorMessage = "Password must be at least 8 characters long.")]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare("Password",ErrorMessage = "Password and its confirmation do not match.")]
		public string ConfirmPassword { get; set; }
	}
}
