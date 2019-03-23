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
    public class ShowController : Controller
    {
        private readonly IShowRepository _repo;

        public ShowController(IShowRepository repo)
        {
            _repo = repo;
        }

        //Get all shows
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _repo.GetAllShows();

            //return new ObjectResult(result);
            return View("AllShows", await _repo.GetAllShows());
        }

        //Get single show
        [HttpGet("{id}")]
        public async Task<ActionResult<Show>> Get(long id)
        {
            var show = await _repo.GetShow(id);

            if (show == null)
            {
                return new NotFoundResult();
            }
            return new ObjectResult(show);
        }

        //Get show edit form
        [HttpGet]
        public IActionResult CreateShow()
        {
            Show show = new Show();
            return View("CreateShow", show);
        }

        //Store posted new show in database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateShow([FromForm] Show show)
        {
            show.Id = await _repo.GetNextId();
            await _repo.CreateShow(show);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditShow(long id)
        {
            Show show = _repo.GetShow(id).Result;
            return View("EditShow", show);
        }

        //Store edited show in database
        [HttpPost]
        public async Task<ActionResult<Show>> PutShow([FromForm] Show show)
        {
            long id = show.Id;

            var showFromDb = await _repo.GetShow(id);

            if (showFromDb == null)
                return new NotFoundResult();

            show.Id = showFromDb.Id;
            show.InternalId = showFromDb.InternalId;

            await _repo.UpdateShow(show);

            return RedirectToAction("Index"); 
        }

        //Delete sected show from database
        [HttpGet]
        public async Task<IActionResult> DeleteShow( long id)
        {
            var post = await _repo.GetShow(id);

            if (post == null)
                return new NotFoundResult();

            await _repo.DeleteShow(id);

            return RedirectToAction("Index");
            //return new OkResult();
        }
    }
}