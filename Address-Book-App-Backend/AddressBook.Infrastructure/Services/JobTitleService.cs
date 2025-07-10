using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddressBook.Application.DTOs;
using AddressBook.Domain.Entities;

using AddressBook.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using AddressBook.Application.Interfaces;

namespace AddressBook.Infrastructure.Services
{
    public class JobTitleService : IJobTitleService
    {
        private readonly AppDbContext _context;

        public JobTitleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<JobTitle> CreateAsync(CreateJobTitleDto dto)
        {
            var jobTitle = new JobTitle
            {
                Name = dto.Name
            };

            _context.JobTitles.Add(jobTitle);
            await _context.SaveChangesAsync();

            return new JobTitle
            {
                Id = jobTitle.Id,
                Name = jobTitle.Name
            };
        }

        public async Task<List<JobTitle>> GetAllAsync()
        {
            return await _context.JobTitles
                .Include(j => j.AddressBookEntries)
                .ToListAsync();
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var jobTitle = await _context.JobTitles.FindAsync(id);
            if (jobTitle == null) return false;

            _context.JobTitles.Remove(jobTitle);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(UpdateJobTitleDto dto)
        {
            var jt = await _context.JobTitles.FindAsync(dto.Id);
            if (jt == null) return false;
            jt.Name = dto.Name;
            await _context.SaveChangesAsync();
            return true;
        }



    }

}
