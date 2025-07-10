using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBook.Domain.Entities
{
    public class AddressBookEntry
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Address { get; set; }
        public string? MobileNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhotoUrl { get; set; }

        public int JobTitleId { get; set; }
        public JobTitle JobTitle { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public int Age { get; set; }  
    }
}
