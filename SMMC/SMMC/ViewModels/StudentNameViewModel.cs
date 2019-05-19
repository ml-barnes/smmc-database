using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SMMC.ViewModels
{
    public class StudentNameViewModel
    {
        [Display(Name = "Student")]
        public int id { get; set; }
        public string name { get; set; }
    }
}
