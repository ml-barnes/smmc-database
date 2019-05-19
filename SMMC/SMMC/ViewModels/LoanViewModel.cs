using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMMC.Models;
using System.ComponentModel.DataAnnotations;

namespace SMMC.ViewModels
{
    public class LoanViewModel
    {
        public int InstrumentInventoryId { get; set; }
        public int EnrollmentId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOut { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateIn { get; set; }

        public Enrollment Enrollment { get; set; }
        public InstrumentInventory InstrumentInventory { get; set; }
    }
}
