using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Market_Store___First_Project.Models
{
    public partial class Home
    {
        public decimal Id { get; set; }
        public string Slide1 { get; set; }
        public string Slide2 { get; set; }
        public string Slide3 { get; set; }
        public string OurFeatures1 { get; set; }
        public string Websitename { get; set; }
        public string Logoimage { get; set; }
        public string OurFeatures2 { get; set; }
        public string OurFeatures3 { get; set; }

        [NotMapped]
       public virtual IFormFile Slide1File { get; set; }
        [NotMapped]
        public virtual IFormFile Slide2File { get; set; }
        [NotMapped]
        public virtual IFormFile Slide3File { get; set; }
    }
}
