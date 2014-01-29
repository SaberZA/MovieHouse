using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MovieHouse.Logic;

namespace MovieHouse.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/
        private static MovieRequest _movieRequest;

        public ActionResult Index(string searchQuery)
        {
            if (String.IsNullOrEmpty(searchQuery))
                return View(new List<Movie>());

            _movieRequest = new MovieRequest(searchQuery);
            _movieRequest.DownloadJsonMovieResponse();
            return View(_movieRequest.Movies);
        }

        public ActionResult Movie(string id)
        {
            var movie = _movieRequest.Movies.FirstOrDefault(m => m.id == id);
            return View(movie ?? new Movie());
        }

    }
}
