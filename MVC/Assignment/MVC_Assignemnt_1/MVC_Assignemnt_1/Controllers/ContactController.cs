using System.Threading.Tasks;
using System.Web.Mvc;
using MVC_Assignemnt_1.Models;
using MVC_Assignemnt_1.Repository;

namespace MVC_Assignemnt_1.Controllers
{
    public class ContactsController : Controller
    {
        private readonly IContactRepository repo = new ContactRepository();

        public async Task<ActionResult> Index()
        {
            var contacts = await repo.GetAllAsync();
            return View(contacts);
        }


        public ActionResult Create()
        {
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Contact contact)
        {
            if (ModelState.IsValid)
            {
                await repo.CreateAsync(contact);
                return RedirectToAction("Index");
            }
            return View(contact);
        }

       
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null) return HttpNotFound();

            var contact = await repo.GetAllAsync();
            var item = contact.Find(c => c.Id == id.Value);

            if (item == null) return HttpNotFound();
            return View(item);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            await repo.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
