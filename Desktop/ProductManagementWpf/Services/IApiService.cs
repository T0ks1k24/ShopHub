using ProductManagementWpf.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagementWpf.Services
{
    public interface IApiService
    {
        Task<AuthResponse> LoginAsync(LoginRequest loginRequest);
        Task<AuthResponse> RegisterAsync(RegisterRequest registerRequest);
    }
}
