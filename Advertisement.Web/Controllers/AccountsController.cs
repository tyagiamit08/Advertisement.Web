using System.Threading.Tasks;
using Advertisement.Web.Models.Accounts;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Advertisement.Web.Controllers
{
	public class AccountsController : Controller
	{
		private readonly SignInManager<CognitoUser> _signInManager;
		private readonly UserManager<CognitoUser> _userManager;
		private readonly CognitoUserPool _pool;

		public AccountsController(SignInManager<CognitoUser> singInManager, UserManager<CognitoUser> userManager,
			CognitoUserPool pool)
		{
			_signInManager = singInManager;
			_userManager = userManager;
			_pool = pool;
		}

		[HttpGet]
		public async Task<IActionResult> SignUp()
		{
			var model = new SignUpModel();
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> SignUp(SignUpModel model)
		{
			if (ModelState.IsValid)
			{
				var user = _pool.GetUser(model.Email);
				if (user.Status != null)
				{
					ModelState.AddModelError("UserExists", "User with this Email already exists.");
					return View(model);
				}

				user.Attributes.Add(CognitoAttribute.Name.ToString(), model.Email);
				var createdUser = await _userManager.CreateAsync(user, model.Password);

				if (createdUser.Succeeded)
				{
					return RedirectToAction("Confirm");
				}
			}

			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Confirm()
		{
			var model = new ConfirmModel();
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Confirm(ConfirmModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);

				if (user == null)
				{
					ModelState.AddModelError("NotFound", "User with this Email not found.");
					return View(model);
				}

				var result = await ((CognitoUserManager<CognitoUser>)_userManager).ConfirmSignUpAsync(user, model.Code, true);

				if (result.Succeeded)
				{
					return RedirectToAction("Index", "Home");
				}
				else
				{
					foreach (var identityError in result.Errors)
					{
						ModelState.AddModelError(identityError.Code, identityError.Description);
					}

					return View(model);
				}
			}

			return View(model);
		}
	}
}
