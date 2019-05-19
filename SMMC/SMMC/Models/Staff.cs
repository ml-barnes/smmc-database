using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class Staff
    {
        public Staff()
        {
            Payroll = new HashSet<Payroll>();
            Technician = new HashSet<Technician>();
            Tutor = new HashSet<Tutor>();
        }

        public int StaffId { get; set; }
        public int PersonId { get; set; }
        public int ContactId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? LeftDate { get; set; }
        public int Hours { get; set; }

        public Contact Contact { get; set; }
        public Person Person { get; set; }
        public ICollection<Payroll> Payroll { get; set; }
        public ICollection<Technician> Technician { get; set; }
        public ICollection<Tutor> Tutor { get; set; }
    }
}
