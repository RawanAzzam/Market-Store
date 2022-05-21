using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Market_Store___First_Project.Models
{
    public partial class UserLogin
    {
        public decimal Id { get; set; }
        public string UserName { get; set; }
        public string Passwordd { get; set; }
        public decimal? RoleId { get; set; }
        public decimal? UserId { get; set; }
        public bool? IsVerfiy { get; set; }

        public virtual Role Role { get; set; }
        public virtual Systemuser User { get; set; }
    }
}
