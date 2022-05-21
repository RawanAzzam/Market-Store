using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Market_Store___First_Project.Models
{
    public partial class Card
    {
        public DateTime? Expiredate { get; set; }
        public string Tcb { get; set; }
        public decimal? Balance { get; set; }
        public decimal Id { get; set; }
    }
}
