using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class Loan
    {
        public int InstrumentInventoryId { get; set; }
        public int EnrollmentId { get; set; }
        public DateTime DateLoaned { get; set; }
        public DateTime? DateReturned { get; set; }

        public Enrollment Enrollment { get; set; }
        public InstrumentInventory InstrumentInventory { get; set; }
    }
}
