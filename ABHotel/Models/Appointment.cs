using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ABHotel.Models
{
    public class Appointment
    {
        public int ID { get; set; }

        [Display(Name = "Reception")] //new
        public string ReceptionId { get; set; }

        [ForeignKey("ReceptionId")] //new
        public virtual ApplicationUser Reception { get; set; }

        public DateTime AppointmentDate { get; set; }

        [NotMapped] //not add to DB
        public DateTime AppointmentTime { get; set; }

        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public bool isConfirmed { get; set; }

    }
}
