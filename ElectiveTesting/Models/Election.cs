using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ElectiveTesting.Models
{
    public class Election
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string HostId { get; set; }

        public virtual ICollection<Elective> Electives { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}