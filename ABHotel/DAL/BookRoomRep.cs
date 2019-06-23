using ABHotel.Data;
using ABHotel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABHotel.DAL
{
    public class BookRoomRep : BaseRepository
    {
        public BookRoomRep(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public List<RoomSelectedForAppointment> GetSelApp(int id) => _db.RoomSelectedForAppointments.Where(p => p.AppointmentId == id).ToList();
        internal void Add(Appointment appointments)
        {
            _db.Appointments.Add(appointments);
            _db.SaveChanges();
        }
    }
}
