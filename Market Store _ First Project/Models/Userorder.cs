using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Market_Store___First_Project.Models
{
    public partial class Userorder
    {
        public Userorder()
        {
            Productorder = new HashSet<Productorder>();
        }

        public DateTime? Dateoforder { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Userid { get; set; }
        public bool? IsCheckout { get; set; }
        public decimal Id { get; set; }

        public virtual Systemuser User { get; set; }
        public virtual ICollection<Productorder> Productorder { get; set; }
    }
}
