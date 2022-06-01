using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Market_Store___First_Project.Models
{
    public partial class Aboutus
    {
        public string Info { get; set; }
        public decimal Id { get; set; }

        public string OurFeatures1 { get; set; }
        public string OurFeatures2 { get; set; }
        public string OurFeatures3 { get; set; }

        public string ImagePath { get; set; }

        [NotMapped]
        public virtual IFormFile ImageFile { get; set; }
    }
}
