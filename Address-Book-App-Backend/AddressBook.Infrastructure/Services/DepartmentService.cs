using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddressBook.Application.DTOs;
using AddressBook.Domain.Entities;
using AddressBook.Application.Interfaces;
using AddressBook.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Infrastructure.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly AppDbContext _context;

        public DepartmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Department> CreateAsync(CreateDepartmentDto dto)
        {
            var department = new Department
            {
                Name = dto.Name
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return new Department
            {
                Id = department.Id,
                Name = department.Name
            };
        }

        public async Task<List<Department>> GetAllAsync()
        {
            var departments = await _context.Departments
                .Include(d => d.AddressBookEntries)
                .Select(d => new Department
                {
                    Id = d.Id,
                    Name = d.Name,
                    AddressBookEntries = d.AddressBookEntries 
                })
                .ToListAsync();

            return departments;
        }
        public async Task DeleteAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
                throw new KeyNotFoundException("Department not found");

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> UpdateAsync(UpdateDepartmentDto dto)
        {
            var department = await _context.Departments.FindAsync(dto.Id);
            if (department == null)
                return false;

            department.Name = dto.Name;
            await _context.SaveChangesAsync();
            return true;
        }


    }
}
