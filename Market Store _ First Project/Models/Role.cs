using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Market_Store___First_Project.Models
{
    public partial class Role
    {
        public Role()
        {
            UserLogin = new HashSet<UserLogin>();
        }

        public decimal Id { get; set; }
        public string Rolename { get; set; }

        public virtual ICollection<UserLogin> UserLogin { get; set; }
    }
}
