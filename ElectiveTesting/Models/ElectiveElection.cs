using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ElectiveTesting.Models
{
    public class ElectiveElection
    {
        [Key]
        [ForeignKey("Election")]
        [Column(Order = 1)]
        public int ElectionId { get; set; }
        [Key]
        [ForeignKey("Elective")]
        [Column(Order = 2)]
        public int ElectiveId { get; set; }

        public virtual Election Election { get; set; }
        public virtual Elective Elective { get; set; }
    }
}