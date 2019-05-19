using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SMMC.Models;
using System.ComponentModel;

namespace SMMC.ViewModels
{
    public class StudentCreateViewModel
    {
        //Student
        public int StudentId { get; set; }
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
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Person { get; set; }
        public string Contact { get; set; }

        //Guardian
        public int GuardianId { get; set; }
        public int? GuardianContactId { get; set; }
        [Required]
        [DisplayName("Guardian First Name")]
        public string GuardianFirstName { get; set; }
        [Required]
        [DisplayName("Guardian Last Name")]
        public string GuardianLastName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Guardian Date of Birth")]
        public DateTime GuardianDob { get; set; }
        [Required]
        [DisplayName("Guardian Address")]
        public string GuardianAddress { get; set; }
        [Required]
        [DisplayName("Guardian Email")]
        public string GuardianEmail { get; set; }
        [Required]
        [DisplayName("Guardian Phone")]
        public string GuardianPhone { get; set; }
        public string GuardianContact { get; set; }

        //StudentGuardian
        public int StudentGuardianStudentId { get; set; }
        public int StudentGuardianGuardianId { get; set; }

        public int GuardianDropDown { get; set; }

        public List<string> Enrollments { get; set; }

        public Boolean guardian { get; set; }
    }
}
