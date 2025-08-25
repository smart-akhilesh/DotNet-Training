using System.Data.Entity;

namespace MVC_Assignemnt_1.Models
{
    public class ContactContext : DbContext
    {
        public ContactContext() : base("ContactDB")
        {
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}
