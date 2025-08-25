using System.Data.Entity;

namespace CodeChallenge_9.Models
{
    public class MoviesDbContext : DbContext
    {
        public MoviesDbContext() : base("MoviesDB") { }

        public DbSet<Movie> Movies { get; set; }
    }
}
