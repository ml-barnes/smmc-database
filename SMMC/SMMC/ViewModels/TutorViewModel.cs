using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SMMC.ViewModels
{
    public class TutorViewModel
    {
        [Display(Name = "Tutor")]
        public int id { get; set; }
        public string name { get; set; }
    }
}
