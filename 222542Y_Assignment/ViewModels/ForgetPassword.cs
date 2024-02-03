using System.ComponentModel.DataAnnotations;

namespace _222542Y_Assignment.ViewModels
{
    public class ForgetPassword
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
