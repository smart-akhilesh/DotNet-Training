using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models

{

    public class ForgotPasswordViewModel

    {

        [Required(ErrorMessage = "Username or Email is required")]

        [Display(Name = "Username or Email")]

        public string UsernameOrEmail { get; set; }

        [Display(Name = "OTP")]

        public string OTP { get; set; }

        [DataType(DataType.Password)]

        [Display(Name = "New Password")]

        public string NewPassword { get; set; }

        [DataType(DataType.Password)]

        [Display(Name = "Confirm Password")]

        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]

        public string ConfirmPassword { get; set; }

        public bool IsStep2 { get; set; } = false;

        public string Message { get; set; }

    }

}

