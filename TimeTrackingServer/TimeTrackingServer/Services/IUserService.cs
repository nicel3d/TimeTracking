using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackingServer.Models;

namespace TimeTrackingServer.Services
{
    public class SecurityTokenUser
    {
        public string token { get; set; }
        public string userFullName { get; set; }
    }

    public interface IUserService
    {
        Task<SecurityTokenUser> Authenticate(string username, string password);
        List<UserModel> GetAll();
    }
}
