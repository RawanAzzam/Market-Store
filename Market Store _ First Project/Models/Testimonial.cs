using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Market_Store___First_Project.Models
{
    public partial class Testimonial
    {
        public string Username { get; set; }
        public string Imagepath { get; set; }
        public string Content { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Userid { get; set; }
        public decimal Id { get; set; }

        public virtual Systemuser User { get; set; }
    }
}
