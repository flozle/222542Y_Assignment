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
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^&*!])[A-Za-z\d@#$%^&*!]{12,}$",
			ErrorMessage = "Password must be 12 characters long and contain at least 1 lowercase, 1 uppercase, 1 numeric and 1 special character")]
		public string Password { get; set; }
		[Required]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
		public string ConfirmPassword { get; set; }
		[Required]
		[DataType(DataType.Upload)]
		[AllowedExtensions(new string[] { ".jpg" }, ErrorMessage = "Only .jpg files allowed")]
		public IFormFile Photo { get; set; }

		//captcha
		public string Token { get; set; }
	}
	public class AllowedExtensionsAttribute : ValidationAttribute
	{
		private readonly string[] _extensions;

		public AllowedExtensionsAttribute(string[] extensions)
		{
			_extensions = extensions;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value == null)
				return ValidationResult.Success;

			IFormFile file = value as IFormFile;
			string extension = System.IO.Path.GetExtension(file.FileName);

			if (file != null)
			{
				if (!_extensions.Contains(extension.ToLower()))
				{
					return new ValidationResult(GetErrorMessage());
				}
			}

			return ValidationResult.Success;
		}

		public string GetErrorMessage()
		{
			return $"This file extension is not allowed!";
		}
	}

}
