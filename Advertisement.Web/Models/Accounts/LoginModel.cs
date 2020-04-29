﻿using System.ComponentModel.DataAnnotations;

namespace Advertisement.Web.Models.Accounts
{
	public class LoginModel
	{
		[Required(ErrorMessage = "Email Address is required.")]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password is required.")]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Display(Name = "Remember Me")]
		public bool RememberMe { get; set; }
	}
}
