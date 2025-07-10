using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddressBook.Application.DTOs;
using FluentValidation;

namespace AddressBook.Application.Validators
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
