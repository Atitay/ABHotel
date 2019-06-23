using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ABHotel.DAL;
using ABHotel.Data;
using ABHotel.Models;
using ABHotel.Models.ViewModel;
using ABHotel.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ABHotel.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.SuperAdminEndUser)] //แยกว่าใครสามารถดูหน้านี้ได้ พวก แก้ไข ลบ
    [Area("Admin")]
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _db; //DI
        private readonly HostingEnvironment _hostingEnvironment; //get img
        private RoomRep roomRep; //Rep
        private RoomTypesRep roomTypesRep;

        [BindProperty]
        public RoomsViewModel RoomVM { get; set; }
        public RoomsController(ApplicationDbContext db, HostingEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
            roomRep = new RoomRep(_db);
            roomTypesRep = new RoomTypesRep(_db);

            RoomVM = new RoomsViewModel()
            {
                RoomTypes = roomTypesRep.GetRoomTypes,
                Rooms = new Models.Rooms()
            };
        }

        public async Task<IActionResult> Index()
        {
            var rooms = roomRep.GetRoom;
            return View(rooms);
        }

        //Get: Create
        public IActionResult Create()
        {
            return View(RoomVM);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]

        //POST Create 
        public async Task<IActionResult> CreatePost()
        {
            if (!ModelState.IsValid)
                return View(RoomVM);

            roomRep.Add(RoomVM.Rooms);


            string webRootPath = _hostingEnvironment.WebRootPath; //logic use img ,set path
            var files = HttpContext.Request.Form.Files; //set file = file upload

            var roomsFromDb = _db.Rooms.Find(RoomVM.Rooms.ID); //get by Id from Room

            if (files.Count != 0)  /*have Img*/
            {
                //Image has been uploaded
                var uploads = Path.Combine(webRootPath, SD.ImageFolder); //set path to folder
                var extension = Path.GetExtension(files[0].FileName); //set name img
                //find obj, copy upload to server
                using (var filestream = new FileStream(Path.Combine(uploads, RoomVM.Rooms.ID + extension), FileMode.Create))
                {
                    files[0].CopyTo(filestream);
                }
                roomsFromDb.Image = @"\" + SD.ImageFolder + @"\" + RoomVM.Rooms.ID + extension;   //save here, set Path
            }
            else  /*no Img*/
            {
                //when user does not upload image
                var uploads = Path.Combine(webRootPath, SD.ImageFolder + @"\" + SD.DefaultRoomImage); //no upload but save default
                System.IO.File.Copy(uploads, webRootPath + @"\" + SD.ImageFolder + @"\" + RoomVM.Rooms.ID + ".png");
                roomsFromDb.Image = @"\" + SD.ImageFolder + @"\" + RoomVM.Rooms.ID + ".png";
            }
            roomRep.Save();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            
                return NotFound();
           
            RoomVM.Rooms = await _db.Rooms.Include(m => m.RoomTypes).SingleOrDefaultAsync(m => m.ID == id);

            if (RoomVM.Rooms == null)
            
                return NotFound();
          
            return View(RoomVM);
        }

        //Post : Edit
        [HttpPost]
        [ValidateAntiForgeryToken]      //secure ASP.NET
        public async Task<IActionResult> Edit(int id)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostingEnvironment.WebRootPath;  //logic use img
                var files = HttpContext.Request.Form.Files;
                
                var roomsFromDb = roomRep.FindDB(RoomVM.Rooms);

                //find old img -->delete ,add new img

                if (files.Count > 0 && files[0] != null)  // img exist
                {
                    //if user uploads new image
                    var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                    var extension_new = Path.GetExtension(files[0].FileName);

                    var extension_old = Path.GetExtension(roomsFromDb.Image);

                    if (System.IO.File.Exists(Path.Combine(uploads, RoomVM.Rooms.ID + extension_old)))
                    {
                        System.IO.File.Delete(Path.Combine(uploads, RoomVM.Rooms.ID + extension_old));
                    }

                    using (var filestream = new FileStream(Path.Combine(uploads, RoomVM.Rooms.ID + extension_new), FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }

                    RoomVM.Rooms.Image = @"\" + SD.ImageFolder + @"\" + RoomVM.Rooms.ID + extension_new;   //save here
                }
                if (RoomVM.Rooms.Image != null) //have img

                {
                    roomsFromDb.Image = RoomVM.Rooms.Image;
                }

                roomsFromDb.Name = RoomVM.Rooms.Name;
                roomsFromDb.Price = RoomVM.Rooms.Price;
                roomsFromDb.Available = RoomVM.Rooms.Available;
                roomsFromDb.RoomTypeId = RoomVM.Rooms.RoomTypeId;
                roomsFromDb.Location = RoomVM.Rooms.Location;

                roomRep.Save();
                return RedirectToAction(nameof(Index));

            }

            return View(RoomVM);
        }

        //Get : Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            
                return NotFound();
            
            
            RoomVM.Rooms = await _db.Rooms.Include(m => m.RoomTypes).SingleOrDefaultAsync(m => m.ID == id);

            if (RoomVM.Rooms == null)
            
                return NotFound();
            
            return View(RoomVM);
        }

        //Get : Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            
                return NotFound();
            

            RoomVM.Rooms = await _db.Rooms.Include(m => m.RoomTypes).SingleOrDefaultAsync(m => m.ID == id);

            if (RoomVM.Rooms == null)
            
                return NotFound();
            
            return View(RoomVM);
        }

        //Post : Delete
        [HttpPost, ActionName("Delete")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            
            Rooms rooms = roomRep.Find(id);

            if (rooms == null)
            {
                return NotFound();
            }
            else
            {
                var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                var extnsion = Path.GetExtension(rooms.Image);

                if (System.IO.File.Exists(Path.Combine(uploads, rooms.ID + extnsion)))
                {
                    System.IO.File.Delete(Path.Combine(uploads, rooms.ID + extnsion));
                }

                roomRep.Delete(rooms);
                return RedirectToAction(nameof(Index));
            }

        }
    }
}