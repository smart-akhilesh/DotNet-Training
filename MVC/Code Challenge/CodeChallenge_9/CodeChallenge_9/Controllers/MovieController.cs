using System.Web.Mvc;
using CodeChallenge_9.Models;

namespace CodeChallenge_9.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieRepository repo = new MovieRepository();


        public ActionResult Index()
        {
            var movies = repo.GetAll();
            return View(movies);
        }

      
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                repo.Add(movie);
                repo.Save();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

       
        public ActionResult Edit(int? id)
        {
            if (id == null) return HttpNotFound();

            var movie = repo.GetById(id.Value);
            if (movie == null) return HttpNotFound();

            return View(movie);
        }

        [HttpPost]
        public ActionResult Edit(Movie movie)
        {
            if (ModelState.IsValid)
            {
                repo.Update(movie);
                repo.Save();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

 
        public ActionResult Delete(int? id)
        {
            if (id == null) return HttpNotFound();

            var movie = repo.GetById(id.Value);
            if (movie == null) return HttpNotFound();

            return View(movie);
        }


        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            repo.Delete(id);
            repo.Save();
            return RedirectToAction("Index");
        }


        public ActionResult ByYear(int year)
        {
            ViewBag.Year = year; 
            var movies = repo.GetByYear(year);
            return View(movies);
        }

   
        public ActionResult ByDirector(string name)
        {
            ViewBag.DirectorName = name; 
            var movies = repo.GetByDirector(name);
            return View(movies);
        }
    }
}
