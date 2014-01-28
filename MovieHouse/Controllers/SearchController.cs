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

        public ActionResult Index(string searchQuery)
        {
            if (searchQuery == null)
                return View(new List<Movie>());

            MovieRequest movieRequest = new MovieRequest(searchQuery);
            movieRequest.DownloadJsonMovieResponse();
            return View(movieRequest.Movies);
        }

    }
}
