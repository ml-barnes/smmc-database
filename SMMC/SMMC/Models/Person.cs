using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class Person
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }

        public Guardian Guardian { get; set; }
        public Staff Staff { get; set; }
        public Student Student { get; set; }
    }
}
