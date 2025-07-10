using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddressBook.Application.DTOs;
using AddressBook.Domain.Entities;

namespace AddressBook.Application.Interfaces
{
    public interface IJobTitleService
    {
        Task<JobTitle> CreateAsync(CreateJobTitleDto dto);
        Task<List<JobTitle>> GetAllAsync();

        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(UpdateJobTitleDto dto);

    }
}
