using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Market_Store___First_Project.Models
{
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            Product = new HashSet<Product>();
        }

        public string Name { get; set; }
        public string ImagePath { get; set; }
        public decimal Id { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}
