using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddressBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<AddressBookEntry> AddressBookEntries { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<Department> Departments { get; set; }

        public DbSet<AppUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
            modelBuilder.Entity<JobTitle>()
                .Property(j => j.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("int");  
            modelBuilder.Entity<JobTitle>()
                .Property(j => j.Name)
                .IsRequired()
                .HasMaxLength(100);

            
            modelBuilder.Entity<Department>()
                .Property(d => d.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("int"); 
            modelBuilder.Entity<Department>()
                .Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);

            
            modelBuilder.Entity<AddressBookEntry>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("int");  
            modelBuilder.Entity<AddressBookEntry>()
                .Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<AddressBookEntry>()
                .HasOne(e => e.JobTitle)
                .WithMany(j => j.AddressBookEntries)
                .HasForeignKey(e => e.JobTitleId);

            modelBuilder.Entity<AddressBookEntry>()
                .HasOne(e => e.Department)
                .WithMany(d => d.AddressBookEntries)
                .HasForeignKey(e => e.DepartmentId);
        }

    }
}
