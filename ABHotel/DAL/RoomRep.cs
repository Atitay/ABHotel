using ABHotel.Data;
using ABHotel.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABHotel.DAL
{
    public class RoomRep : BaseRepository
    {
        public RoomRep(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        internal void Add(Rooms rooms)
        {
            _db.Rooms.Add(rooms);
            _db.SaveChangesAsync();
        }

        public List<Rooms> GetRoom => _db.Rooms.Include(r => r.RoomTypes).ToList();

        internal void Save() => _db.SaveChangesAsync();

        internal void Delete(Rooms rooms)
        {
            _db.Rooms.Remove(rooms);
            _db.SaveChangesAsync();
        }

        internal Rooms Find(int id) => _db.Rooms.Find(id);

        internal Rooms FindDB(Rooms rooms) =>  _db.Rooms.Where(m => m.ID == rooms.ID).FirstOrDefault();

       
    }
}
