using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ABHotel.Models
{
    public class RoomSelectedForAppointment
    {
        public int ID { get; set; }
        public int AppointmentId { get; set; }

        [ForeignKey("AppointmentId")]
        public virtual Appointment Appointment { get; set; }

        public int RoomId { get; set; }

        [ForeignKey("RoomId")]
        public virtual Rooms Room { get; set; }
    }
}
