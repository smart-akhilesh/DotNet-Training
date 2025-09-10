using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace ECommerce.Models
{
    public class ForgotPassword
    {  // [Required(ErrorMessage ="Username Required")]
        public string Username { get; set; }
        //[Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }
       // [Required(ErrorMessage = "Required")]
        public string ConfirmPassword { get; set; }

    }
}