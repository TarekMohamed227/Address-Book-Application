using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBook.Domain.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<AddressBookEntry> AddressBookEntries { get; set; } = new List<AddressBookEntry>();
    }
}
