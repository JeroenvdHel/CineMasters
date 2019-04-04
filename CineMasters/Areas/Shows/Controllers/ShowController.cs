using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinemasters.Helpers;
using CineMasters.Areas.Shows.Models;
using CineMasters.Areas.Shows.Models.ViewModels;
using CineMasters.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CineMasters.Areas.Shows.Controllers
{
    [Area("Shows")]
    public class ShowController : Controller
    {
        private readonly IShowRepository _showRepo;
        private readonly IMovieRepository _movieRepo;
        private readonly IRoomRepository _roomRepo;
        public int PageSize = 10;
        public string SortedBy { get; set; } = "date";

        public ShowController(IShowRepository showRepo, IMovieRepository movieRepo, IRoomRepository roomRepo)
        {
            _showRepo = showRepo;
            _movieRepo = movieRepo;
            _roomRepo = roomRepo;
        }

        //Get all shows
        [HttpGet]
        public async Task<IActionResult> Index(int showPage = 1, string sortedBy = "default", string filter = "none")
        {
            if (SortedBy != sortedBy && sortedBy != "default")
            {
                SortedBy = sortedBy;
            }
            ViewBag.sortedBy = SortedBy;

            var shows = await _showRepo.GetAllShows();
            var showList = SortShows(CreateShowListViewModel(shows, showPage, sortedBy), sortedBy);
            showList.Shows = showList.Shows.Skip((showPage - 1) * PageSize).Take(PageSize);


            return View("AllShows", showList);
        }

        [HttpPost]
        public async Task<IActionResult> GetShowsByFilter(int showPage = 1, string sortedBy = "date", string filter = "")
        {
            var movies = _movieRepo.GetAllMovies(filter).Result;

            if (SortedBy != sortedBy && sortedBy != "default")
            {
                SortedBy = sortedBy;
            }
            ViewBag.sortedBy = SortedBy;

            var shows = await _showRepo.GetAllShows();
            var showList = SortShows(CreateShowListViewModel(shows, showPage, sortedBy), sortedBy);
            showList.Shows = showList.Shows.Skip((showPage - 1) * PageSize).Take(PageSize);


            return View("AllShows", showList);
        }

        //Get single show
        [HttpGet]
        public async Task<ActionResult<Show>> Get(long id)
        {
            var show = await _showRepo.GetShow(id);

            if (show == null)
            {
                return new NotFoundResult();
            }
            return new ObjectResult(show);
        }

        //Get show create form
        [Authorize(Roles = "Administrator, Back-office medewerker")]
        [HttpGet]
        public IActionResult CreateShow()
        {
            CreateSelectListForMovieAndRoom();

            Show show = new Show();
            return View("CreateShow", show);
        }

        //Store posted new show in database
        [Authorize(Roles = "Administrator, Back-office medewerker")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateShow([FromForm] Show show)
        {
            show.Id = await _showRepo.GetNextId();
            await _showRepo.CreateShow(show);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator, Back-office medewerker")]
        [HttpGet]
        public IActionResult EditShow(long id)
        {
            CreateSelectListForMovieAndRoom();

            Show show = _showRepo.GetShow(id).Result;
            return View("EditShow", show);
        }

        //Store edited show in database
        [Authorize(Roles = "Administrator, Back-office medewerker")]
        [HttpPost]
        public async Task<ActionResult<Show>> PutShow([FromForm] Show show)
        {
            long id = show.Id;

            var showFromDb = await _showRepo.GetShow(id);

            if (showFromDb == null)
                return new NotFoundResult();

            show.Id = showFromDb.Id;
            show.InternalId = showFromDb.InternalId;

            bool success = await _showRepo.UpdateShow(show);

            if (!success)
                return new ObjectResult("Niet gelukt om de show te updaten!");

            return RedirectToAction("Index"); 


        }

        //Delete sected show from database
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> DeleteShow(long id)
        {
            var post = await _showRepo.GetShow(id);

            if (post == null)
                return new NotFoundResult();

            await _showRepo.DeleteShow(id);

            return RedirectToAction("Index");
        }



        /// <summary>
        /// Create 2 lists to populate select tags with lists of rooms and movies and 
        /// store them in a ViewBag.
        /// </summary>
        private void CreateSelectListForMovieAndRoom() {
            var movieList = _movieRepo.GetAllMovies().Result;
            SelectList movieSelect = new SelectList(
                    movieList.Select(x => new { Value = x.Id, Text = x.Title }),
                    "Value",
                    "Text"
                );
            ViewBag.ListOfMovies = movieSelect;

            var roomList = _roomRepo.GetAllRooms().Result;
            SelectList roomsSelect = new SelectList(
                    roomList.Select(x => new { Value = x.Id, Text = x.Name }),
                    "Value",
                    "Text"
                );
            ViewBag.ListOfRooms = roomsSelect;
        }

        private ShowListViewModel CreateShowListViewModel(IEnumerable<Show> shows, int showPage, string sortedBy)
        {
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = showPage,
                ItemsPerPage = PageSize,
                TotalItems = shows.Count(),
                SortedBy = sortedBy
            };
            ShowListViewModel list = new ShowListViewModel
            {
                Shows = shows,
                PagingInfo = pagingInfo
            };

            return list;
        }

        private ShowListViewModel SortShows(ShowListViewModel showList, string sortedBy)
        {
            switch (sortedBy)
            {
                case "movie":
                    showList.Shows = showList.Shows.OrderBy(s => s.Movie.Id);
                    break;
                case "room":
                    showList.Shows = showList.Shows.OrderBy(s => s.Room.Id);
                    break;
                case "date":
                    showList.Shows = showList.Shows.OrderBy(o => o.DateTime);
                    break;
                default:
                    showList.Shows = showList.Shows.OrderBy(o => o.DateTime);
                    break;
            }
            return showList;
        }
    }
}