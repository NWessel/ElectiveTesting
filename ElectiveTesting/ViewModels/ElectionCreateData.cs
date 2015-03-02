using ElectiveTesting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectiveTesting.ViewModels
{
    public class ElectionCreateData
    {
        public Election election { get; set; }
        public IEnumerable<string> availableUsers { get; set; }
        public IEnumerable<string> invitedUsers { get; set; }
    }
}