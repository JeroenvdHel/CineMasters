using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineMasters.Models.Domain;
using CineMasters.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CineMasters.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieRepository _repo;

        public MovieController(IMovieRepository repo)
        {
            _repo = repo;
        }

        //GET movie
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            return View("AllMovies", await _repo.GetAllMovies());
        }

        //GET movie/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> Get(long id)
        {
            var movie = await _repo.GetMovie(id);

            if (movie == null)
            {
                return new NotFoundResult();
            }
            return new ObjectResult(movie);
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
        public async void CreateMovie([FromForm] Movie movie)
        {
            movie.Id = await _repo.GetNextId();
            await _repo.Create(movie);
            RedirectToAction("Index");
        }

        //PUT movie/1
        [HttpPut("{id}")]
        public async Task<ActionResult<Movie>> Put(long id, [FromForm] Movie movie)
        {
            var movieFromDb = await _repo.GetMovie(id);

            if (movieFromDb == null)
                return new NotFoundResult();

            movie.Id = movieFromDb.Id;
            movie.InternalId = movieFromDb.InternalId;

            await _repo.Update(movie);

            return new OkObjectResult(movie);
        }

        //DELETE movie/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var post = await _repo.GetMovie(id);

            if (post == null)
                return new NotFoundResult();

            await _repo.Delete(id);

            return new OkResult();
        }
    }
}