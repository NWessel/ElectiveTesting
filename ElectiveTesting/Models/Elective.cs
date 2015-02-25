using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectiveTesting.Models
{
    public class Elective
    {
        public int ElectiveId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Election> Elections { get; set; }
    }
}