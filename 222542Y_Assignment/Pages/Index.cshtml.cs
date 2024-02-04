using Authentication.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using WebApp_Core_Identity.Model;

namespace _222542Y_Assignment.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;
        private UserManager<User> userManager { get; }
        private SignInManager<User> signInManager { get; }
        private UserDbContext dbContext { get; }
        private IHttpContextAccessor _httpContextAccessor { get; }

        public IndexModel(ILogger<IndexModel> logger, UserManager<User> userManager, SignInManager<User> signInManager, UserDbContext dbContext, IHttpContextAccessor httpContextAccessor)
		{
			_logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
		}

		public async Task OnGetAsync()
		{
			var id = _httpContextAccessor.HttpContext.Session.GetString("ID");

            if (id == null)
			{
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    await HttpContext.SignOutAsync("MyCookieUser");
                    _httpContextAccessor.HttpContext.Session.Clear();
                    return;
                }
                // audit
                var audit = new AuditLog()
                {
                    Email = user.Email,
                    Action = "Timeout",
                    Timestamp = DateTime.Now
                };
                dbContext.AuditLogs.Add(audit);
                await dbContext.SaveChangesAsync();

                // sign out
                _httpContextAccessor.HttpContext.Session.Clear();
                await HttpContext.SignOutAsync("MyCookieUser");
                await signInManager.SignOutAsync();
                Response.Redirect("/Login");
                return;
            }
            if (!_httpContextAccessor.HttpContext.Session.IsAvailable)
            {
                _httpContextAccessor.HttpContext.Session.Clear();
                await HttpContext.SignOutAsync("MyCookieUser");
                Response.Redirect("/Login");
                return;
            }
		}

    }
}