using _222542Y_Assignment.ViewModels;
using Authentication.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;
using WebApp_Core_Identity.Model;

namespace _222542Y_Assignment.Pages
{
    public class LoginModel : PageModel
    {
		private SignInManager<User> signInManager { get; }
		private GoogleCaptchaService _captchaService { get; }
		private UserManager<User> userManager { get; }
		private UserDbContext dbContext { get; }

		[BindProperty]
        public Login LModel { get; set; }

        public LoginModel(SignInManager<User> signInManager, GoogleCaptchaService captchaService, UserDbContext dbContext)
        {
            this.signInManager = signInManager;
            _captchaService = captchaService;
			this.dbContext = dbContext;
		}
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //verify response token with google
            var captchaResult = await _captchaService.ValidateToken(LModel.Token);
            if (!captchaResult)
            {
                return Page();
            }

            if (ModelState.IsValid)
            {
				var user = await userManager.FindByEmailAsync(LModel.Email);

				if (user != null)
				{
					var identityResult = await signInManager.PasswordSignInAsync(LModel.Email, LModel.Password, LModel.RememberMe, lockoutOnFailure: true);

					if (identityResult.Succeeded)
					{
						var audit = new AuditLog()
						{
							Email = LModel.Email,
							Action = "Login",
							Timestamp = DateTime.Now
						};
						dbContext.AuditLogs.Add(audit);
						await dbContext.SaveChangesAsync();
						await userManager.UpdateAsync(user);
						return RedirectToPage("Index");
					}

					// Account lockout
					if (identityResult.IsLockedOut)
					{
						ModelState.AddModelError("", "Account locked out");
					}
					else
					{
						ModelState.AddModelError("", "Username or Password incorrect");
					}
					// End of account lockout
				}
				//end of account lockout
			}
            return Page();
        }
    }
}
