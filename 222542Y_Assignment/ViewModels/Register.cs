using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace _222542Y_Assignment.ViewModels
{
	public class Register
	{
		[Required]
		[DataType(DataType.Text)]
		[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
		public string FirstName { get; set; }
		[Required]
		[DataType(DataType.Text)]
		[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
		public string LastName { get; set; }
		[Required]
		[DataType(DataType.CreditCard)]
		[RegularExpression(@"^(\d{16})$", ErrorMessage = "Invalid Credit Card Number")]
		public string CreditCard { get; set; }
		[Required]
		[DataType(DataType.PhoneNumber)]
		[RegularExpression(@"^(\d{8})$", ErrorMessage = "Invalid Phone Number")]
		public string PhoneNumber { get; set; }
		[Required]
		[DataType(DataType.Text)]
		[RegularExpression(@"^[a-zA-Z0-9\s,.'-]{3,}$", ErrorMessage = "Invalid Address")]
		public string BillingAddress { get; set; }
		[Required]
		[DataType(DataType.Text)]
		[RegularExpression(@"^[a-zA-Z0-9\s,.'-]{3,}$", ErrorMessage = "Invalid Address")]
		public string ShippingAddress { get; set; }
		[Required]
		[DataType(DataType.EmailAddress)]
		[RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid Email Address")]
		public string Email { get; set; }
		[Required]
		[DataType(DataType.Password)]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^&*!])[A-Za-z\d@#$%^&*!]{12,}$", ErrorMessage = "Password must be 8-15 characters long and contain at least 1 lowercase, 1 uppercase, 1 numeric and 1 special character")]
		public string Password { get; set; }
		[Required]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
		public string ConfirmPassword { get; set; }
		//to be added photo

		//captcha
		public string Token { get; set; }
	}
}
