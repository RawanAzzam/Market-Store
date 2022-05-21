using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Market_Store___First_Project.Models
{
    public partial class Product
    {
        public Product()
        {
            ProductStore = new HashSet<ProductStore>();
            Rate = new HashSet<Rate>();
        }

        public decimal Id { get; set; }
        public string Namee { get; set; }
        public decimal? Sale { get; set; }
        public decimal? Price { get; set; }
        public string ImagePath { get; set; }
        public decimal ProductCategoryId { get; set; }
        public DateTime? DateOfAdd { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }
        public virtual ICollection<ProductStore> ProductStore { get; set; }
        public virtual ICollection<Rate> Rate { get; set; }
    }
}
