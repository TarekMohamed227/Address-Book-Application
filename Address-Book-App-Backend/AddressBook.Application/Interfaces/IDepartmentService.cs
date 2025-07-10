using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddressBook.Application.DTOs;
using AddressBook.Domain.Entities;

namespace AddressBook.Application.Interfaces
{
    public interface IDepartmentService
    {

        Task<Department> CreateAsync(CreateDepartmentDto dto);
        Task<List<Department>> GetAllAsync();
        Task DeleteAsync(int id);

        Task<bool> UpdateAsync(UpdateDepartmentDto dto);

    }
}
