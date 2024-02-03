using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _222542Y_Assignment.Pages
{
	[Authorize(Policy = "MustBelongToHRDepartment", AuthenticationSchemes = " MyCookieUser")]
	public class PrivacyModel : PageModel
	{
		private readonly ILogger<PrivacyModel> _logger;

		public PrivacyModel(ILogger<PrivacyModel> logger)
		{
			_logger = logger;

		}

		public void OnGet()
		{
		}
	}
}