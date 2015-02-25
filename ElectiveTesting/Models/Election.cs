using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectiveTesting.Models
{
    public class Election
    {
        public int ElectionId { get; set; }
        public int Name { get; set; }

        public virtual ICollection<Elective> Electives { get; set; }

    }
}