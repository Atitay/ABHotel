using ABHotel.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABHotel.DAL
{
    public class AdminUserRep : BaseRepository
    {
        public AdminUserRep(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
