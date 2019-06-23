using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABHotel.DAL;
using ABHotel.Data;
using ABHotel.Models;
using ABHotel.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ABHotel.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.SuperAdminEndUser)] //แยกว่าใครสามารถดูหน้านี้ได้ พวก แก้ไข ลบ
    [Area("Admin")]
    public class RoomTypesController : Controller
    {
        private readonly ApplicationDbContext _db;
        private RoomTypesRep roomTypesRep;


        public RoomTypesController(ApplicationDbContext db)
        {
            _db = db;
            roomTypesRep = new RoomTypesRep(_db);
        }
        public IActionResult Index()
        {
            return View(roomTypesRep.GetRoomTypes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        //POST Create Action  Method
        public async Task<IActionResult> Create(RoomTypes roomTypes)
        {
            if (ModelState.IsValid)
            {
                roomTypesRep.Add(roomTypes);
                return RedirectToAction(nameof(Index));
            }
            return View(roomTypes);
        }

        //Get: Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var roomType = roomTypesRep.Find(id);

            if (roomType == null)
                return NotFound();

            return View(roomType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        //POST Edit
        public async Task<IActionResult> Edit(int id, RoomTypes roomTypes)
        {
            if (id != roomTypes.ID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                roomTypesRep.Update(roomTypes);
                return RedirectToAction(nameof(Index));
            }
            return View(roomTypes);
        }

        //Get: Detail
        public async Task<IActionResult> Details(int? id)
        {
            var roomType = roomTypesRep.Find(id);

            if (roomType == null)
                return NotFound();
            
            return View(roomType);
        }

        //Get: Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)            
                return NotFound();
            var roomType = roomTypesRep.Find(id);

            if (roomType == null)
                return NotFound();

            return View(roomType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        //POST Delete 
        public async Task<IActionResult> Delete(int id)
        {
            var roomTypes = roomTypesRep.Find(id);

            roomTypesRep.Delete(roomTypes);
            return RedirectToAction(nameof(Index));
        }
    }
}