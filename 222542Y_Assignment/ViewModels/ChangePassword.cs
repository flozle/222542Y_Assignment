using System.ComponentModel.DataAnnotations;

namespace _222542Y_Assignment.ViewModels
{
	public class ChangePassword
	{
		[Required]
		[DataType(DataType.Password)]
		public string OldPassword { get; set; }
		[Required]
		[DataType(DataType.Password)]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^&*!])[A-Za-z\d@#$%^&*!]{12,}$", ErrorMessage = "Password must be 8-15 characters long and contain at least 1 lowercase, 1 uppercase, 1 numeric and 1 special character")]
		public string NewPassword { get; set; }
		[Required]
		[DataType(DataType.Password)]
		[Compare(nameof(NewPassword), ErrorMessage = "Password and confirmation password does not match")]
		public string ConfirmPassword { get; set; }
		[Required]
		public string Token { get; set; }
	}
}
