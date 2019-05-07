using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTrackingServer.Models;

namespace TimeTrackingServer.Services
{
    public class SecurityTokenUser
    {
        public string Token { get; set; }
        public string UserFullName { get; set; }
    }

    public interface IUserService
    {
        Task<SecurityTokenUser> Authenticate(string username, string password);
        List<UserModel> GetAll();
    }
}
