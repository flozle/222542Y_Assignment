using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using WebApp_Core_Identity.Model;

namespace _222542Y_Assignment.Pages
{
	[Authorize]
	public class LogoutModel : PageModel
    {
		private readonly SignInManager<User> signInManager;
		private UserDbContext dbContext { get; }
		private UserManager<User> userManager { get; }
        private IHttpContextAccessor _httpContextAccessor { get; }
        public LogoutModel(SignInManager<User> signInManager, UserDbContext dbContext, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
		{
			this.signInManager = signInManager;
			this.dbContext = dbContext;
			this.userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
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
			dbContext.AuditLogs.Add(audit);
			await dbContext.SaveChangesAsync();

            _httpContextAccessor.HttpContext.Session.Clear();

			await HttpContext.SignOutAsync("MyCookieUser");
            //session
            _httpContextAccessor.HttpContext.Session.SetString("ID", Guid.NewGuid().ToString());
            await signInManager.SignOutAsync();
			return RedirectToPage("Login");
		}
		public async Task<IActionResult> OnPostDontLogoutAsync()
		{
			return RedirectToPage("Index");
		}
	}
}
