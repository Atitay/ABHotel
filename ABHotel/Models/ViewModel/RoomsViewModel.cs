using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABHotel.Models.ViewModel
{
    public class RoomsViewModel
    {
        public Rooms Rooms { get; set; }
        public IEnumerable<RoomTypes> RoomTypes { get; set; }
    }
}
