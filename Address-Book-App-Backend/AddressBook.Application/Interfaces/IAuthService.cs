using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddressBook.Application.DTOs;

namespace AddressBook.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
    }
}
