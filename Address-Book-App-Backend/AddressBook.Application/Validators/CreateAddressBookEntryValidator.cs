using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddressBook.Application.DTOs;
using FluentValidation;

namespace AddressBook.Application.Validators
{
    public class CreateAddressBookEntryValidator : AbstractValidator<CreateAddressBookEntryDto>
    {
        public CreateAddressBookEntryValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
            RuleFor(x => x.MobileNumber).NotEmpty().Matches(@"^\+?[0-9]{10,15}$");
            RuleFor(x => x.DateOfBirth).NotEmpty();
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.JobTitleId).NotEmpty();
            RuleFor(x => x.DepartmentId).NotEmpty();
        }
    }
}
