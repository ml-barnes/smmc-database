using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SMMC.ViewModels
{
    public class SheetMusicViewModel
    {
        [Display(Name = "Sheet Music")]
        public int id { get; set; }
        public string name { get; set; }


    }
}
