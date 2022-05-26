using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Market_Store___First_Project.Models
{
    public partial class Systemuser
    {
        public Systemuser()
        {
            Rate = new HashSet<Rate>();
            Report = new HashSet<Report>();
            Testimonial = new HashSet<Testimonial>();
            UserLogin = new HashSet<UserLogin>();
            Userorder = new HashSet<Userorder>();
        }

        public string Username { get; set; }
        public string Email { get; set; }
        public decimal Id { get; set; }
        public string Location { get; set; }
        public string ImagePath { get; set; }
        [NotMapped]
        public virtual IFormFile ImageFile { get; set; }
        public virtual ICollection<Rate> Rate { get; set; }
        public virtual ICollection<Report> Report { get; set; }
        public virtual ICollection<Testimonial> Testimonial { get; set; }
        public virtual ICollection<UserLogin> UserLogin { get; set; }
        public virtual ICollection<Userorder> Userorder { get; set; }
    }
}
