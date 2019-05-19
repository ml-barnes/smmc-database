using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class Contact
    {
        public Contact()
        {
            Student = new HashSet<Student>();
        }

        public int ContactId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public Guardian Guardian { get; set; }
        public LocalMusicians LocalMusicians { get; set; }
        public Staff Staff { get; set; }
        public ICollection<Student> Student { get; set; }
    }
}
