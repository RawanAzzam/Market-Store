using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Market_Store___First_Project.Models
{
    public partial class Productorder
    {
        public decimal? Orderid { get; set; }
        public decimal? Productid { get; set; }
        public decimal? Quntity { get; set; }
        public decimal Id { get; set; }

        public virtual Userorder Order { get; set; }
        public virtual ProductStore Product { get; set; }
    }
}
