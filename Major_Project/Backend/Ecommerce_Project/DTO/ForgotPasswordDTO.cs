using System.ComponentModel.DataAnnotations;
namespace Ecommerce_Project.DTO
{
    public class ForgotPasswordDTO
    {
        [Required(ErrorMessage = "Username or Email is required")]
        public string UsernameOrEmail { get; set; }

        // OTP will be null or empty in the first step (request OTP)
        public string OTP { get; set; }

        // Passwords will be null or empty in the first step
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}