using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SMMC.ViewModels
{
    public class EnrollmentDisplayViewModel
    {
        [Display(Name = "Enrollment")]
        public int id { get; set; }
        [Display(Name = "Enrollment")]
        public string enrollment { get; set; }
    }
}
