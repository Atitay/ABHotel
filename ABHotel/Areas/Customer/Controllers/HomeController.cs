using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ABHotel.Models;
using ABHotel.Data;
using Microsoft.EntityFrameworkCore;
using ABHotel.Extension;
using ABHotel.DAL;

namespace ABHotel.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly HomeRep homeRep;
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
            homeRep = new HomeRep(_db);
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Room(int? id)
        {
            List<Rooms> roomsList;

            if (id == null)
            
                roomsList = homeRep.GetRoomList;
            
            else
            
                roomsList = homeRep.GetRoomListById(id);
            
            return View(roomsList);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var room = await _db.Rooms.Include(m => m.RoomTypes).Where(m => m.ID == id).FirstOrDefaultAsync();

            return View(room);
        }

        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetailsPost(int id)
        {
            List<int> lstBookRoom = HttpContext.Session.Get<List<int>>("ssBookRoom");

            if (lstBookRoom == null)
            {
                lstBookRoom = new List<int>();
            }

            lstBookRoom.Add(id);
            HttpContext.Session.Set("ssBookRoom", lstBookRoom);

            return RedirectToAction("Room", "Home", new { area = "Customer" });
        }

        public IActionResult Remove(int id)
        {
            List<int> lstBookRoom = HttpContext.Session.Get<List<int>>("ssBookRoom");
            if (lstBookRoom.Count > 0)
            {
                if (lstBookRoom.Contains(id))
                {
                    lstBookRoom.Remove(id); //remove
                }
            }
            HttpContext.Session.Set("ssBookRoom", lstBookRoom);
            return RedirectToAction(nameof(Room));
            ;
        }

        //public IActionResult About(int id)
        //{
        //    return View();
        //}

        public IActionResult Contact(int id)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

