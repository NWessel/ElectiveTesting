using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectiveTesting.Models
{
    public class Vote
    {
        public int VoteId { get; set; }
        public int ElectionId { get; set; }
        public int ElectiveId { get; set; }
        public int ApplicationUserId { get; set; }

        public virtual Election Election { get; set; }
        public virtual Elective Elective { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}