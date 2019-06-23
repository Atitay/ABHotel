using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ABHotel.Models
{
    public class ApplicationUser : IdentityUser
    {

        [Display(Name = "Reception")]
        public string Name { get; set; }

        [NotMapped]  //no map, not add to Db
        public bool isSuperAdmin { get; set; }
    }
}
