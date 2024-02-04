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
        private IHttpContextAccessor _httpContextAccessor { get; }

        [BindProperty]
        public Login LModel { get; set; }

        public LoginModel(SignInManager<User> signInManager, GoogleCaptchaService captchaService, UserDbContext dbContext, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.signInManager = signInManager;
            _captchaService = captchaService;
			this.dbContext = dbContext;
			this.userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
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
					var identityResult = await signInManager.PasswordSignInAsync(LModel.Email, LModel.Password, false, lockoutOnFailure: true);

					if (identityResult.Succeeded)
					{
						var user = await userManager.FindByEmailAsync(LModel.Email);
						var claims = new List<Claim> { };
                        var audit = new AuditLog()
						{
							Email = LModel.Email,
							Action = "Login",
							Timestamp = DateTime.Now
						};
						dbContext.AuditLogs.Add(audit);
						await dbContext.SaveChangesAsync();
                    if (LModel.Email == "staff@gmail.com") { 
						claims = new List<Claim>
							{
								new Claim("Department", "HR")
							};
						}
							else
						{
							claims = new List<Claim>
								{
								};
						}
						var i = new ClaimsIdentity(claims, "MyCookieUser");
						ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(i);
						await HttpContext.SignInAsync("MyCookieUser", claimsPrincipal);

						//session
						_httpContextAccessor.HttpContext.Session.SetString("ID", Guid.NewGuid().ToString());

                    if (user?.LastPasswordChangedDate.AddMinutes(2) < DateTime.Now)
						{
							// Password has expired
							// Redirect user to change password page
							return RedirectToPage("PasswordExpired");
							
						}
						else
						{
							return RedirectToPage("Index");
						}
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
            return Page();
        }
    }
}
