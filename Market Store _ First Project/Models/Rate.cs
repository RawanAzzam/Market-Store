using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Market_Store___First_Project.Models
{
    public partial class Rate
    {
        public decimal Id { get; set; }
        public decimal? UserId { get; set; }
        public decimal? ProductId { get; set; }
        public bool? RateNum { get; set; }
        public string Feedback { get; set; }

        public virtual Product Product { get; set; }
        public virtual Systemuser User { get; set; }
    }
}
