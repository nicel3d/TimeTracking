using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackingServer.Models;

namespace TimeTrackingServer.Services
{
    public interface IUserService
    {
        UserModel Authenticate(string username, string password);
        List<UserModel> GetAll();
    }
}
