using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ABHotel.DAL;
using ABHotel.Data;
using ABHotel.Models;
using ABHotel.Models.ViewModel;
using ABHotel.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ABHotel.Areas.Admin.Controllers
{

    [Authorize(Roles = SD.AdminEndUser + "," + SD.SuperAdminEndUser)]
    [Area("Admin")]

    public class AppointmentController : Controller
    {

        private readonly ApplicationDbContext _db;

        private readonly AppointmentRep appointmentRep;
        //new
        //private int PageSize = 5;
        public AppointmentController(ApplicationDbContext db)
        {
            _db = db;
            appointmentRep = new AppointmentRep(_db);
        }

        public async Task<IActionResult> Index(int productPage = 1, string searchName = null,
                                                string searchEmail = null,
                                                string searchPhone = null,
                                                string searchDate = null)
        {

            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            AppointmentViewModel appointmentVM = new AppointmentViewModel()
            {
                Appointments = new List<Models.Appointment>()
            };

            //new
            StringBuilder param = new StringBuilder();

            param.Append("/Admin/Appointment?productPage=:");

            param.Append("&searchName=");
            if (searchName != null)
            {
                param.Append(searchName);
            }

            param.Append("&searchEmail=");
            if (searchEmail != null)
            {
                param.Append(searchEmail);
            }

            param.Append("&searchPhone=");
            if (searchPhone != null)
            {
                param.Append(searchPhone);
            }

            param.Append("&searchDate=");
            if (searchDate != null)
            {
                param.Append(searchDate);
            }
            //end new


            appointmentVM.Appointments = _db.Appointments
               .Include(a => a.Reception).ToList();

            if (User.IsInRole(SD.AdminEndUser))
            {
                appointmentVM.Appointments = appointmentVM.Appointments
                    .Where(a => a.ReceptionId == claim.Value).ToList();
            }

            //all Search
            if (searchName != null)
            {
                appointmentVM.Appointments = appointmentVM.Appointments
                    .Where(a => a.CustomerName.ToLower().Contains(searchName.ToLower())).ToList();
            }

            if (searchEmail != null)
            {
                appointmentVM.Appointments = appointmentVM.Appointments
                    .Where(a => a.CustomerEmail.ToLower().Contains(searchEmail.ToLower())).ToList();
            }

            if (searchPhone != null)
            {
                appointmentVM.Appointments = appointmentVM.Appointments
                    .Where(a => a.CustomerPhone.ToLower().Contains(searchPhone.ToLower())).ToList();
            }

            if (searchDate != null)
            {
                try
                {
                    DateTime appDate = Convert.ToDateTime(searchDate);
                    appointmentVM.Appointments = appointmentVM.Appointments
                                       .Where(a => a.AppointmentDate.ToShortDateString().Equals(appDate.ToShortDateString())).ToList();
                }
                catch (Exception ex)
                {

                }
            }

            //new
            //var count = appointmentVM.Appointments.Count;
            //appointmentVM.Appointments = appointmentVM.Appointments
            //    .OrderBy(p => p.AppointmentDate)
            //            .Skip((productPage - 1) * PageSize)
            //            .Take(PageSize).ToList();

            //appointmentVM.PagingInfo = new PagingInfo
            //{
            //    CurrentPage = productPage,
            //    ItemsPerPage = PageSize,
            //    TotalItems = count,
            //    urlParam = param.ToString()
            //}; 
            //end new

            return View(appointmentVM);
        }

        //Get: Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomList = (IEnumerable<Rooms>)(from p in _db.Rooms
                                                join a in _db.RoomSelectedForAppointments
                                                on p.ID equals a.RoomId
                                                where a.AppointmentId == id
                                                select p).Include("RoomTypes");

            AppointmentDetailViewModel objAppointmentVM = new AppointmentDetailViewModel()
            {
                Appointments = _db.Appointments.Include(a => a.Reception)
                                                .Where(a => a.ID == id).FirstOrDefault(),
                Reception = _db.ApplicationUsers.ToList(),
                Rooms = roomList.ToList()
            };
            return View(objAppointmentVM);
        }

        //Post: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, AppointmentDetailViewModel objAppointmentVM)
        {
            if (ModelState.IsValid)
            {
                objAppointmentVM.Appointments.AppointmentDate = objAppointmentVM.Appointments.AppointmentDate
                                                .AddHours(objAppointmentVM.Appointments.AppointmentTime.Hour)
                                                .AddMinutes(objAppointmentVM.Appointments.AppointmentTime.Minute);

                var appointmentFromDb = _db.Appointments.Where(a => a.ID == objAppointmentVM.Appointments.ID).FirstOrDefault();

                appointmentFromDb.CustomerName = objAppointmentVM.Appointments.CustomerName;
                appointmentFromDb.CustomerEmail = objAppointmentVM.Appointments.CustomerEmail;
                appointmentFromDb.CustomerPhone = objAppointmentVM.Appointments.CustomerPhone;
                appointmentFromDb.AppointmentDate = objAppointmentVM.Appointments.AppointmentDate;
                appointmentFromDb.isConfirmed = objAppointmentVM.Appointments.isConfirmed;

                if (User.IsInRole(SD.SuperAdminEndUser))
                {
                    appointmentFromDb.ReceptionId = objAppointmentVM.Appointments.ReceptionId;
                }

                _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(objAppointmentVM);
        }

        //Get: DetailS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)

                return NotFound();


            var roomList = (IEnumerable<Rooms>)(from p in _db.Rooms
                                                join a in _db.RoomSelectedForAppointments
                                                on p.ID equals a.RoomId
                                                where a.AppointmentId == id
                                                select p).Include("RoomTypes");

            AppointmentDetailViewModel objAppointmentVM = new AppointmentDetailViewModel()
            {
                Appointments = _db.Appointments.Include(a => a.Reception)
                                                .Where(a => a.ID == id).FirstOrDefault(),
                Reception = _db.ApplicationUsers.ToList(),
                Rooms = roomList.ToList()
            };
            return View(objAppointmentVM);
        }

        //Get: Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomList = (IEnumerable<Rooms>)(from p in _db.Rooms
                                                join a in _db.RoomSelectedForAppointments
                                                on p.ID equals a.RoomId
                                                where a.AppointmentId == id
                                                select p).Include("RoomTypes");

            AppointmentDetailViewModel objAppointmentVM = new AppointmentDetailViewModel()
            {
                Appointments = _db.Appointments.Include(a => a.Reception).Where(a => a.ID == id).FirstOrDefault(),
                Reception = _db.ApplicationUsers.ToList(),
                Rooms = roomList.ToList()
            };
            return View(objAppointmentVM);
        }

        //Port: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _db.Appointments.FindAsync(id);  //find

            _db.Appointments.Remove(appointment); //remove
            await _db.SaveChangesAsync(); //save

            return RedirectToAction(nameof(Index)); //back to Index
        }

    }
}