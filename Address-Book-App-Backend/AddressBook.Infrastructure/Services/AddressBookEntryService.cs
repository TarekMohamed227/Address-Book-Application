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
    public class AddressBookEntryService : IAddressBookEntryService
    {
        private readonly string _uploadRoot;
        private readonly AppDbContext _context;

        public AddressBookEntryService(AppDbContext context)
        {
            _context = context;
            _uploadRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        }

        public async Task<int> CreateAsync(CreateAddressBookEntryDto dto)
        {
            string? photoPath = null;

            if (dto.Photo != null && dto.Photo.Length > 0)
            {
                if (!Directory.Exists(_uploadRoot))
                    Directory.CreateDirectory(_uploadRoot);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Photo.FileName)}";
                var fullPath = Path.Combine(_uploadRoot, fileName);

               
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await dto.Photo.CopyToAsync(stream);
                }

                photoPath = $"/uploads/{fileName}";
            }

            var entry = new AddressBookEntry
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Password = dto.Password,
                MobileNumber = dto.MobileNumber,
                Address = dto.Address,
                DateOfBirth = dto.DateOfBirth,
                Age = CalculateAge(dto.DateOfBirth),
                JobTitleId = dto.JobTitleId,
                DepartmentId = dto.DepartmentId,
                PhotoUrl = photoPath
            };

            _context.AddressBookEntries.Add(entry);

            
            await _context.SaveChangesAsync();

            return entry.Id;
        }

        public async Task<AddressBookEntry> GetByIdAsync(int id)
        {
            var entry = await _context.AddressBookEntries
                .Where(e => e.Id == id)
                .Select(e => new AddressBookEntry
                {
                    Id = e.Id,
                    FullName = e.FullName,
                    Email = e.Email,
                    MobileNumber = e.MobileNumber,
                    DateOfBirth = e.DateOfBirth,
                    Address = e.Address,
                    JobTitleId = e.JobTitleId,
                    DepartmentId = e.DepartmentId,
                    PhotoUrl = e.PhotoUrl
                })
                .FirstOrDefaultAsync();

            return entry;
        }

        public async Task<List<AddressBookEntry>> GetAllAsync()
        {
            return await _context.AddressBookEntries
                .Include(x => x.JobTitle)
                .Include(x => x.Department)
                .OrderBy(x => x.FullName)
                .ToListAsync();
        }


        private int CalculateAge(DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if (dob.Date > today.AddYears(-age)) age--;
            return age;
        }

        //public async Task<bool> UpdateAsync(UpdateAddressBookEntryDto dto)
        //{
        //    var entry = await _context.AddressBookEntries.FindAsync(dto.Id);
        //    if (entry == null) return false;

        //    // Check if the department exists
        //    var departmentExists = await _context.Departments
        //        .AnyAsync(d => d.Id == dto.DepartmentId); // Using DepartmentId here
        //    if (!departmentExists)
        //    {
        //        throw new Exception($"Department with ID {dto.DepartmentId} does not exist.");
        //    }

        //    // Update the entry with new values
        //    entry.FullName = dto.FullName;
        //    entry.Email = dto.Email;
        //    entry.Password = dto.Password;
        //    entry.MobileNumber = dto.MobileNumber;
        //    entry.DateOfBirth = dto.DateOfBirth;
        //    entry.Address = dto.Address;
        //    entry.JobTitleId = dto.JobTitleId;  // Using JobTitleId here
        //    entry.DepartmentId = dto.DepartmentId; // Updating the departmentId
        //    entry.Age = CalculateAge(dto.DateOfBirth);

        //    // If there's a photo, update the photo as well
        //    if (dto.Photo != null)
        //    {
        //        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Photo.FileName)}";
        //        var fullPath = Path.Combine(_uploadRoot, fileName);
        //        using var stream = new FileStream(fullPath, FileMode.Create);
        //        await dto.Photo.CopyToAsync(stream);
        //        entry.PhotoUrl = $"/uploads/{fileName}";
        //    }

        //    var result = await _context.SaveChangesAsync();

        //    return result > 0;
        //}
        public async Task<bool> UpdateAsync(UpdateAddressBookEntryDto dto)
        {
            var entry = await _context.AddressBookEntries.FindAsync(dto.Id);
            if (entry == null) return false;

            
            var departmentExists = await _context.Departments
                .AnyAsync(d => d.Id == dto.DepartmentId);
            if (!departmentExists)
            {
                throw new Exception($"Department with ID {dto.DepartmentId} does not exist.");
            }

            
            entry.FullName = dto.FullName;
            entry.Email = dto.Email;

           
            if (!string.IsNullOrEmpty(dto.Password))
            {
                entry.Password = dto.Password;
            }

            entry.MobileNumber = dto.MobileNumber;
            entry.DateOfBirth = dto.DateOfBirth;
            entry.Address = dto.Address;
            entry.JobTitleId = dto.JobTitleId;
            entry.DepartmentId = dto.DepartmentId;

           
            if (dto.Photo != null)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Photo.FileName)}";
                var fullPath = Path.Combine(_uploadRoot, fileName);
                using var stream = new FileStream(fullPath, FileMode.Create);
                await dto.Photo.CopyToAsync(stream);
                entry.PhotoUrl = $"/uploads/{fileName}";
            }

            var result = await _context.SaveChangesAsync();

            return result > 0;  
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var entry = await _context.AddressBookEntries.FindAsync(id);
            if (entry == null) return false;

            _context.AddressBookEntries.Remove(entry);
            await _context.SaveChangesAsync();
            return true;
        }
    }


}
