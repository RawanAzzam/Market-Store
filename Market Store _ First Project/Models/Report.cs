using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Market_Store___First_Project.Models
{
    public partial class Report
    {
        public decimal Id { get; set; }
        public string Mesaage { get; set; }
        public decimal? Userid { get; set; }
        public decimal? Storeid { get; set; }

        public virtual Store Store { get; set; }
        public virtual Systemuser User { get; set; }
    }
}
