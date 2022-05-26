using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Market_Store___First_Project.Models
{
    public partial class Contactususer
    {
        public decimal Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public decimal? Phonenumber { get; set; }
        public string Message { get; set; }
    }
}
