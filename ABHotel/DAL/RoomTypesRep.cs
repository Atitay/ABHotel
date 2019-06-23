using ABHotel.Data;
using ABHotel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABHotel.DAL
{
    public class RoomTypesRep : BaseRepository

    {
        public RoomTypesRep(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public List<RoomTypes> GetRoomTypes => _db.RoomTypes.ToList();

        
        internal void Add(RoomTypes roomTypes)
        {
            _db.Add(roomTypes);
            _db.SaveChangesAsync();
        }

        internal void Update(RoomTypes roomTypes)
        {
            _db.Update(roomTypes);
            _db.SaveChangesAsync();
        }

        internal void Delete(RoomTypes roomTypes)
        {
            _db.RoomTypes.Remove(roomTypes);
            _db.SaveChangesAsync();
        }

        internal RoomTypes Find(int? id)=> _db.RoomTypes.Find(id);
        
    }
}
