using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class Technician
    {
        public Technician()
        {
            Repairs = new HashSet<Repairs>();
        }

        public int TechnicianId { get; set; }
        public int StaffId { get; set; }

        public Staff Staff { get; set; }
        public ICollection<Repairs> Repairs { get; set; }
    }
}
