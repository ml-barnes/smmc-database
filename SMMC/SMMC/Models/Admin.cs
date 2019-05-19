using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class Admin
    {
        public int AdminId { get; set; }
        public int StaffId { get; set; }

        public Staff Staff { get; set; }
    }
}
