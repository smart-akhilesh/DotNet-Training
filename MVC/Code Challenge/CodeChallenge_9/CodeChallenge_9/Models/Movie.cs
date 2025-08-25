using System;
using System.ComponentModel.DataAnnotations;

namespace CodeChallenge_9.Models
{
    public class Movie
    {
        [Key]
        public int Mid { get; set; }

        [Required]
        public string MovieName { get; set; }

        public string DirectorName { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfRelease { get; set; }
    }
}
