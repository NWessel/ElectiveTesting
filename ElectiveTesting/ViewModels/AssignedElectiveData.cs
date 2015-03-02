using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectiveTesting.ViewModels
{
    public class AssignedElectiveData
    {
        public int ElectiveId { get; set; }
        public string Name { get; set; }
        public bool Assigned { get; set; }
    }
}