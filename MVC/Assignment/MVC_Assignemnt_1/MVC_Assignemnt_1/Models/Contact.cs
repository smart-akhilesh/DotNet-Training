using System.ComponentModel.DataAnnotations;

namespace MVC_Assignemnt_1.Models
{
    public class Contact
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
