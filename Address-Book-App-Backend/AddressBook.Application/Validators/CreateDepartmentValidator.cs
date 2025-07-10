using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddressBook.Application.DTOs;
using FluentValidation;

namespace AddressBook.Application.Validators
{
    public class CreateDepartmentValidator : AbstractValidator<CreateDepartmentDto>
    {
        public CreateDepartmentValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Department name is required")
                .MaximumLength(100).WithMessage("Max 100 characters");
        }
    }
}
