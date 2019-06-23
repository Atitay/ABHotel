using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ABHotel.DAL;
using ABHotel.Data;
using ABHotel.Extension;
using ABHotel.Models;
using ABHotel.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ABHotel.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class BookRoomController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly BookRoomRep bookRoomRep;

        [BindProperty]
        public BookRoomViewModel BookRoomVM { get; set; }

        public BookRoomController(ApplicationDbContext db)
        {
            _db = db;
            bookRoomRep = new BookRoomRep(_db);
            BookRoomVM = new BookRoomViewModel()
            {
                Room = new List<Models.Rooms>()
            };
        }

        //Get Index Shopping Cart
        public async Task<IActionResult> Index()
        {
            List<int> lstBookRoom = HttpContext.Session.Get<List<int>>("ssBookRoom");

            if (lstBookRoom == null)
            {
                return View(BookRoomVM);
            }
            //have lst in shopping card, do this!
            if (lstBookRoom.Count > 0)
            {
                foreach (int bookItem in lstBookRoom)
                {
                    Rooms room = _db.Rooms.Include(p => p.RoomTypes).Where(p => p.ID == bookItem).FirstOrDefault();
                    BookRoomVM.Room.Add(room);
                }
            }

            return View(BookRoomVM);
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public IActionResult IndexPost()
        {
            List<int> lstBookRoom = HttpContext.Session.Get<List<int>>("ssBookRoom");

            BookRoomVM.Appointment.AppointmentDate = BookRoomVM.Appointment.AppointmentDate
                                                            .AddHours(BookRoomVM.Appointment.AppointmentTime.Hour)
                                                            .AddMinutes(BookRoomVM.Appointment.AppointmentTime.Minute);


            Appointment appointments = BookRoomVM.Appointment;

            bookRoomRep.Add(appointments);

            int appointmentId = appointments.ID;

            foreach (int roomId in lstBookRoom)
            {
                RoomSelectedForAppointment roomSelectedForAppointment = new RoomSelectedForAppointment()
                {
                    AppointmentId = appointmentId,
                    RoomId = roomId
                };
                _db.RoomSelectedForAppointments.Add(roomSelectedForAppointment);
            }

            _db.SaveChanges();
            lstBookRoom = new List<int>();
            HttpContext.Session.Set<List<int>>("ssBookRoom", lstBookRoom);

            return RedirectToAction("AppointmentConfirmation", "BookRoom", new { id = appointmentId });
        }

        public IActionResult Remove(int id)
        {
            List<int> lstBookRoom = HttpContext.Session.Get<List<int>>("ssBookRoom");
            //have item, can remove shopping cart
            if (lstBookRoom.Count > 0)
            {
                if (lstBookRoom.Contains(id))
                {
                    lstBookRoom.Remove(id);
                }
            }
            HttpContext.Session.Set("ssBookRoom", lstBookRoom);

            return RedirectToAction(nameof(Index));
        }


        //Get
        public IActionResult AppointmentConfirmation(int id)
        {
            BookRoomVM.Appointment = _db.Appointments.Where(a => a.ID == id).FirstOrDefault();

            List<RoomSelectedForAppointment> objRoomList = bookRoomRep.GetSelApp(id);

            foreach (RoomSelectedForAppointment roomAptObj in objRoomList)
            {
                BookRoomVM.Room.Add(_db.Rooms.Include(p => p.RoomTypes).Where(p => p.ID == roomAptObj.RoomId).FirstOrDefault());
            }
            return View(BookRoomVM);
        }

       

    }

}
