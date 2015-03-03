using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ElectiveTesting.Models
{
    public class Vote
    {
        [Key]
        [ForeignKey("Election")]
        [Column(Order = 1)]
        public int ElectionId { get; set; }
        [ForeignKey("Elective")]
        [Column(Order = 2)]
        public int ElectiveId { get; set; }
        [Key]
        [ForeignKey("ApplicationUser")]
        [Column(Order=3)]
        public string ApplicationUserId { get; set; }

        public virtual Election Election { get; set; }
        public virtual Elective Elective { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}