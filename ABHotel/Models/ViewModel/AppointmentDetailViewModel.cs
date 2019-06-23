using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABHotel.Models.ViewModel
{
    public class AppointmentDetailViewModel
    {
        public Appointment Appointments { get; set; }
        public List<ApplicationUser> Reception { get; set; }
        public List<Rooms> Rooms { get; set; }
    }
}
