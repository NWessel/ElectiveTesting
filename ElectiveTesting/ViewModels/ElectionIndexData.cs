using ElectiveTesting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectiveTesting.ViewModels
{
    public class ElectionIndexData
    {
        public IEnumerable<Election> Elections { get; set; }
        public IEnumerable<Elective> Electives { get; set; }
        public IEnumerable<ApplicationUser> ApplicationUsers { get; set; }
    }
}