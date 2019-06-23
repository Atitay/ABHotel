using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABHotel.Models.ViewModel
{
    public class BookRoomViewModel
    {
        public List<Rooms> Room { get; set; }
        public Appointment Appointment { get; set; }
    }
}
