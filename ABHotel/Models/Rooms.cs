using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ABHotel.Models
{
    public class Rooms
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Room Number")]
        public string Name { get; set; }
        public double Price { get; set; }
        public bool Available { get; set; }
        public string Image { get; set; }
        public string Location { get; set; }

        [Display(Name = "Room Type")]
        public int RoomTypeId { get; set; }

        [ForeignKey("RoomTypeId")]
        public virtual RoomTypes RoomTypes { get; set; }
    }
}
