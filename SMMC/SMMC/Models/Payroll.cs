using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class Payroll
    {
        public int PayrollId { get; set; }
        public int StaffId { get; set; }
        public DateTime Date { get; set; }
        public int Amount { get; set; }
        public string Notes { get; set; }

        public Staff Staff { get; set; }
    }
}
