using ABHotel.Data;
using ABHotel.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABHotel.DAL
{
    public class HomeRep : BaseRepository
    {
        public HomeRep(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public List<Rooms> GetRoomList => _db.Rooms.Include(r => r.RoomTypes).ToList();

        public List<Rooms> GetRoomListById(int? id) =>  _db.Rooms.Where(r => r.RoomTypeId == id).Include(r => r.RoomTypes).ToList();
    }

}
