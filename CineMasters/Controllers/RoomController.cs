using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineMasters.Models.Domain;
using CineMasters.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CineMasters.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomRepository _roomRepo;

        public RoomController(IRoomRepository roomRepo)
        {
            _roomRepo = roomRepo;
        }

        public async Task<IActionResult> Index()
        {
            return View("AllRooms", await _roomRepo.GetAllRooms());
        }

        //GET room/create
        [HttpGet]
        public IActionResult CreateRoom()
        {
            Room room = new Room();
            return View("CreateRoom", room);
        }

        //POST room
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRoom([FromForm] Room room)
        {
            room.Id = await _roomRepo.GetNextId();
            await _roomRepo.CreateRoom(room);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditRoom(long id)
        {
            Room room = _roomRepo.GetRoom(id).Result;
            return View("EditRoom", room);
        }

        //PUT room/1
        [HttpPost]
        public async Task<ActionResult<Room>> PutRoom([FromForm] Room room)
        {
            long id = room.Id;

            var roomFromDb = await _roomRepo.GetRoom(id);

            if (roomFromDb == null)
                return new NotFoundResult();

            room.Id = roomFromDb.Id;
            room.InternalId = roomFromDb.InternalId;

            await _roomRepo.UpdateRoom(room);

            return RedirectToAction("Index");
        }

        //DELETE room/1
        [HttpGet]
        public async Task<IActionResult> DeleteRoom(long id)
        {
            var post = await _roomRepo.GetRoom(id);

            if (post == null)
                return new NotFoundResult();

            await _roomRepo.DeleteRoom(id);

            return RedirectToAction("Index");
            //return new OkResult();
        }
    }
}
