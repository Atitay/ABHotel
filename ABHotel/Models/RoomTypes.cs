using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ABHotel.Models
{
    public class RoomTypes
    {
        public int ID { get; set; }

        [Display(Name = "Room Type")]
        [Required]
        public string Name { get; set; }
    }
}
