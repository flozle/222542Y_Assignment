using _222542Y_Assignment.Core;
using _222542Y_Assignment.ViewModels;
using Authentication.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp_Core_Identity.Model;


namespace _222542Y_Assignment.Pages
{
    public class RegisterModel : PageModel
    {
        private UserManager<User> userManager { get; }
		private SignInManager<User> signInManager { get; }
		private GoogleCaptchaService _captchaService { get;  }
		private UserDbContext dbContext { get; }

		[BindProperty]
		public Register RModel { get; set; }

        public RegisterModel(UserManager<User> userManager, SignInManager<User> signInManager, GoogleCaptchaService captchaService, UserDbContext dbContext)
        {
			this.userManager = userManager;
			this.signInManager = signInManager;
			_captchaService = captchaService;
			this.dbContext = dbContext;
		}

		//Save data into the database
		public async Task<IActionResult> OnPostAsync()
		{
			//verify response token with google
			var captchaResult = await _captchaService.ValidateToken(RModel.Token);
			if (!captchaResult)
			{
				return Page();
			}

			if (ModelState.IsValid)
			{
				var user = new User()
				{
					UserName = RModel.Email,
					FirstName = EncodingClass.Base64Encode(RModel.FirstName),
					LastName = EncodingClass.Base64Encode(RModel.LastName),
					CreditCard = Encryption.Encrypt(RModel.CreditCard),
					PhoneNumber = RModel.PhoneNumber,
					BillingAddress = RModel.BillingAddress,
					ShippingAddress = RModel.ShippingAddress,
					Email = RModel.Email,
				};


				var result = await userManager.CreateAsync(user, RModel.Password);
				if (result.Succeeded)
				{
					var audit = new AuditLog()
					{
						Email = RModel.Email,
						Action = "Register",
						Timestamp = DateTime.Now
					};
					dbContext.AuditLogs.Add(audit);
					await dbContext.SaveChangesAsync();
					await signInManager.SignInAsync(user, false);
					return RedirectToPage("Index");
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}
			return Page();
		}
		public void OnGet()
        {
        }
    }
}
