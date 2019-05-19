using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SMMC.ViewModels
{
    public class StaffViewModel
    {
        public int StaffId { get; set; }
        public int PersonId { get; set; }
        public int? ContactId { get; set; }
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
        public int Hours { get; set; }
        [Required]
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }
        [DisplayName("Left Date")]
        public DateTime? LeftDate { get; set; }
        public string Person { get; set; }
        public string Contact { get; set; }
    }
}
