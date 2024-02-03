using _222542Y_Assignment.Core;
using _222542Y_Assignment.ViewModels;
using Authentication.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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
				var photoPath = "Uploads";
				var fileName = $"{Guid.NewGuid()}{Path.GetExtension(RModel.Photo.FileName)}";
				var filePath = Path.Combine(photoPath, fileName);
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					RModel.Photo.CopyTo(stream);
				}
				var user = new User()
				{
					UserName = RModel.Email,
					FirstName = EncodingClass.Base64Encode(RModel.FirstName),
					LastName = EncodingClass.Base64Encode(RModel.LastName),
					CreditCard = Encryption.Encrypt(RModel.CreditCard),
					PhoneNumber = RModel.PhoneNumber,
					BillingAddress = EncodingClass.Base64Encode(RModel.BillingAddress),
					ShippingAddress = EncodingClass.Base64Encode(RModel.ShippingAddress),
					Email = RModel.Email,
					Photo = filePath,
					OldPassword1 = "",
					OldPassword2 = "",
					OldPassword3 = "",
				};

				//add password to pasword history
                var passwordHash = userManager.PasswordHasher.HashPassword(user, RModel.Password);
                user.OldPassword1 = passwordHash;

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
					var claims = new List<Claim> { };
					if (RModel.Email == "staff@gmail.com")
					{
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
