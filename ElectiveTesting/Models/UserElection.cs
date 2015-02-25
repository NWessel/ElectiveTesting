using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectiveTesting.Models
{
    public class UserElection
    {
        public int UserElectionId { get; set; }
        public int ApplicationUserId { get; set; }
        public int ElectionId { get; set; }
        public bool IsHost { get; set; }

        public virtual Election Election { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        
    }
}