using System;
using System.Collections.Generic;
using System.Text;
using ABHotel.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ABHotel.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<RoomTypes> RoomTypes { get; set; }
        public DbSet<Rooms> Rooms { get; set; }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<RoomSelectedForAppointment> RoomSelectedForAppointments { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    }
}
