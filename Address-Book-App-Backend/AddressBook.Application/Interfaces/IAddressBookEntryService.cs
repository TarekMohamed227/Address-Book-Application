using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddressBook.Application.DTOs;
using AddressBook.Domain.Entities;

namespace AddressBook.Application.Interfaces
{
    public interface IAddressBookEntryService
    {
        Task<int> CreateAsync(CreateAddressBookEntryDto dto);
        Task<AddressBookEntry> GetByIdAsync(int id);
        Task<List<AddressBookEntry>> GetAllAsync();
        Task<bool> UpdateAsync(UpdateAddressBookEntryDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
