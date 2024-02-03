using _222542Y_Assignment.ViewModels;
using Authentication.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp_Core_Identity.Model;

namespace _222542Y_Assignment.Pages
{
	[Authorize]
	public class ChangePasswordModel : PageModel
	{
		private SignInManager<User> signInManager { get; }
		private GoogleCaptchaService _captchaService { get; }
		private UserManager<User> userManager { get; }
		private UserDbContext dbContext { get; }

		[BindProperty]
		public ChangePassword CPModel { get; set; }

		public ChangePasswordModel(UserManager<User> userManager, GoogleCaptchaService captchaService, UserDbContext dbContext)
		{
			this.userManager = userManager;
			_captchaService = captchaService;
			this.dbContext = dbContext;
		}

		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPostAsync()
		{
			//verify response token with google
			var captchaResult = await _captchaService.ValidateToken(CPModel.Token);
			if (!captchaResult)
			{
				return Page();
			}

            var userEmail = User.Identity.Name;

            var user = await userManager.FindByEmailAsync(userEmail);

            var passwordHash = userManager.PasswordHasher.HashPassword(user, CPModel.NewPassword);

            if (userManager.PasswordHasher.VerifyHashedPassword(user, user.OldPassword3, CPModel.NewPassword) == PasswordVerificationResult.Success || userManager.PasswordHasher.VerifyHashedPassword(user, user.OldPassword2, CPModel.NewPassword) == PasswordVerificationResult.Success)
			{
				   ModelState.AddModelError("", "Password cannot be the same as the last 2 passwords");
			}
			else if (userManager.PasswordHasher.VerifyHashedPassword(user, user.OldPassword1, CPModel.NewPassword) == PasswordVerificationResult.Success)
			{
				   ModelState.AddModelError("", "Password cannot be the same as the current password");
			}
			else if (user?.LastPasswordChangedDate.AddMinutes(1) > DateTime.Now)
			{
                ModelState.AddModelError("", "Cannot change password in 1 min");
            }
            else
            {
            var identityResult = await userManager.ChangePasswordAsync(user, CPModel.OldPassword, CPModel.NewPassword);

                if ( identityResult.Succeeded)
                {
					var audit = new AuditLog()
					{
						Email = user.Email,
						Action = "Change Password",
						Timestamp = DateTime.Now
					};
					user.LastPasswordChangedDate = DateTime.Now;
                    user.OldPassword3 = user.OldPassword2;
					user.OldPassword2 = user.OldPassword1;
					user.OldPassword1 = passwordHash;
                    dbContext.AuditLogs.Add(audit);
					await dbContext.SaveChangesAsync();
                    return RedirectToPage("Index");
				}
				else
				{
					ModelState.AddModelError("", "Password incorrect");
				}
            }
            return Page();
		}
	}
}
