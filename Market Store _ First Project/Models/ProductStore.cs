using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Market_Store___First_Project.Models
{
    public partial class ProductStore
    {
        public ProductStore()
        {
            Productorder = new HashSet<Productorder>();
        }

        public decimal Id { get; set; }
        public decimal? Storeid { get; set; }
        public decimal? Productid { get; set; }
        public decimal? Count { get; set; }

        public virtual Product Product { get; set; }
        public virtual Store Store { get; set; }
        public virtual ICollection<Productorder> Productorder { get; set; }
    }
}
