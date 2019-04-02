using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineMasters.Areas.Shows.Models;
using CineMasters.Areas.Shows.Models.ViewModels;
using CineMasters.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CineMasters.Areas.Shows.Controllers
{
    [Area("Shows")]
    public class MovieController : Controller
    {
        private readonly IMovieRepository _movieRepo;
        private readonly IShowRepository _showRepo;

        public MovieController(IMovieRepository movieRepo, IShowRepository showRepo)
        {
            _movieRepo = movieRepo;
            _showRepo = showRepo;
        }

        //GET movie
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            return View("AllMovies", await _movieRepo.GetAllMovies());
        }

        //GET movie/1
        [HttpGet]
        public async Task<ActionResult<Movie>> GetMovieForDetails(long id)
        {
            Movie movie = await _movieRepo.GetMovie(id);
            if (movie == null)
                return new NotFoundResult();

            var shows = await _showRepo.GetShowsForMovie(id);

            List<MovieViewModel.ShowDay> showDays = new List<MovieViewModel.ShowDay>();
            foreach (var show in shows)
            {
                // Store Date, time and ShowId per show in local variables
                DateTime dateTime = show.DateTime;
                DateTime date = dateTime.Date;
                long showId = show.Id;
                MovieViewModel.ShowDay day;

                // Check if there is already a ShowDay object in List<ShowDay> with this shows date. 
                // If so, store this shows time and showId in this object. If not,
                // create a new ShowDay object
                if (showDays.Any(s => s.Date == date))
                {
                    day = showDays.FirstOrDefault(s => s.Date == date);
                    day.ShowTimes.Add(new KeyValuePair<DateTime, long>(dateTime, showId));
                }
                else
                {
                    day = new MovieViewModel.ShowDay(date);
                    day.ShowTimes.Add(new KeyValuePair<DateTime, long>(dateTime, showId));
                    showDays.Add(day);
                }
            }
            MovieViewModel viewModel = new MovieViewModel
            {
                Movie = movie,
                ShowDays = showDays
            };

            return View("MovieDetails", viewModel);
        }

        //GET movie/create
        [HttpGet]
        public IActionResult CreateMovie()
        {
            Movie movie = new Movie();
            return View("CreateMovie", movie);
        }

        //POST movie
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMovie([FromForm] Movie movie)
        {
            movie.Id = await _movieRepo.GetNextId();
            await _movieRepo.Create(movie);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditMovie(long id)
        {
            Movie movie = _movieRepo.GetMovie(id).Result;
            return View("EditMovie", movie);
        }

        //PUT movie/1
        [HttpPost]
        public async Task<ActionResult<Movie>> PutMovie([FromForm] Movie movie)
        {
            long id = movie.Id;

            var movieFromDb = await _movieRepo.GetMovie(id);

            if (movieFromDb == null)
                return new NotFoundResult();

            movie.Id = movieFromDb.Id;
            movie.InternalId = movieFromDb.InternalId;

            await _movieRepo.Update(movie);

            return RedirectToAction("Index"); 
        }

        //DELETE movie/1
        [HttpGet]
        public async Task<IActionResult> DeleteMovie( long id)
        {
            var post = await _movieRepo.GetMovie(id);

            if (post == null)
                return new NotFoundResult();

            await _movieRepo.Delete(id);

            return RedirectToAction("Index");
            //return new OkResult();
        }
    }
}