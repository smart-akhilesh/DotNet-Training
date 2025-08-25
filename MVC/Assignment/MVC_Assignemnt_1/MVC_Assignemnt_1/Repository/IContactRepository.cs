using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MVC_Assignemnt_1.Models;

namespace MVC_Assignemnt_1.Repository
{
    public interface IContactRepository
    {
        Task<List<Contact>> GetAllAsync();
        Task CreateAsync(Contact contact);
        Task DeleteAsync(long id);
    }
}
