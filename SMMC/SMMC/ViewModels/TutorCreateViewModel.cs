using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SMMC.ViewModels
{
    public class TutorCreateViewModel
    {
        //Student
        public int TutorId { get; set; }
        public int StaffId { get; set; }
        [Required]
        [DisplayName("ATCL")]
        public bool Atcl { get; set; }
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required]
        [DisplayName("Date of Birth")]
        public DateTime Dob { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }
        [Required]
        public int Hours { get; set; }
        public string Person { get; set; }
        public string Staff { get; set; }

        public List<string> InstrumentsTeaching { get; set; }
        public List<string> InstrumentsCanTeach { get; set; }
    }
}
