using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AddressBook.Application.DTOs
{
    public class CreateAddressBookEntryDto  
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string MobileNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public int JobTitleId { get; set; }
        public int DepartmentId { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
