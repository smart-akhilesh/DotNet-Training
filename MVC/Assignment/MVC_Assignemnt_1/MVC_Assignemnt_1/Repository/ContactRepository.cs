using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using MVC_Assignemnt_1.Repository;
using MVC_Assignemnt_1.Models;


namespace MVC_Assignemnt_1.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly ContactContext _db = new ContactContext();

        public async Task<List<Contact>> GetAllAsync()
        {
            return await _db.Contacts.ToListAsync();
        }

        public async Task CreateAsync(Contact contact)
        {
            _db.Contacts.Add(contact);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var contact = await _db.Contacts.FindAsync(id);
            if (contact != null)
            {
                _db.Contacts.Remove(contact);
                await _db.SaveChangesAsync();
            }
        }
    }
}
