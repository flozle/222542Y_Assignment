using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp_Core_Identity.Model;

namespace _222542Y_Assignment.Pages
{
    public class LogoutModel : PageModel
    {
		private readonly SignInManager<User> signInManager;
		private UserDbContext dbContext { get; }
		private UserManager<User> userManager { get; }
		public LogoutModel(SignInManager<User> signInManager, UserDbContext dbContext, UserManager<User> userManager)
		{
			this.signInManager = signInManager;
			this.dbContext = dbContext;
			this.userManager = userManager;
		}

		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPostLogoutAsync()
		{
			var user = await userManager.GetUserAsync(User);
			var audit = new AuditLog()
			{
				Email = user.Email,
				Action = "Logout",
				Timestamp = DateTime.Now
			};
			await signInManager.SignOutAsync();
			return RedirectToPage("Login");
		}
		public async Task<IActionResult> OnPostDontLogoutAsync()
		{
			return RedirectToPage("Index");
		}
	}
}
