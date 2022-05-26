using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Market_Store___First_Project.Models
{
    public partial class Store
    {
        public Store()
        {
            ProductStore = new HashSet<ProductStore>();
            Report = new HashSet<Report>();
        }

        public string Storename { get; set; }
        public string Storelocation { get; set; }
        public string Ownername { get; set; }
        public decimal? Categoryid { get; set; }
        public string StoreLogo { get; set; }
        public decimal Id { get; set; }

        [NotMapped]
        public virtual IFormFile LogoFile { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<ProductStore> ProductStore { get; set; }
        public virtual ICollection<Report> Report { get; set; }
    }
}
