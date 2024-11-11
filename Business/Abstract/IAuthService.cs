using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAuthService
    {
        public Task<string> LoginAsync(string Username,string Password);
        public Task<bool> RegisterAsync(string Username, string Password);
        public Task<bool> LogOut(string Username);
    }
}
