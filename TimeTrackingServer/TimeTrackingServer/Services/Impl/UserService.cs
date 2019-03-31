using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TimeTrackingServer.Data;
using TimeTrackingServer.Helpers;
using TimeTrackingServer.Models;

namespace TimeTrackingServer.Services.Impl
{
    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        //private List<UserModel> _users = new List<UserModel>
        //{
        //    new UserModel { Id = Guid.NewGuid(), UserName = "Admin", Email = "test@test.test", Password = "test" }
        //};

        private readonly AppSettings _appSettings;
        private readonly ApplicationDbContext _dbContext;

        public UserService(IOptions<AppSettings> appSettings, ApplicationDbContext dbContext)
        {
            _appSettings = appSettings.Value;
            _dbContext = dbContext;
        }

        public static string SHA512(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);

                // Convert to text
                // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
                var hashedInputStringBuilder = new System.Text.StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
            }
        }

        public async Task<UserModel> Authenticate(string email, string password)
        {
            //_dbContext.User.Add(new UserModel
            //{
            //    Id = Guid.NewGuid(),
            //    Email = email,
            //    Password = SHA512(password),
            //    UserName = "Admin"
            //}
            //);
            //_dbContext.SaveChanges();

            UserModel user = await _dbContext.User.FirstOrDefaultAsync(x => x.Email == email && x.Password == SHA512(password));

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(ClaimTypes.Role, "Admin")
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.Password = null;

            return user;
        }

        public List<UserModel> GetAll()
        {
            return _dbContext.User.Select(x => x).Where(x => x.Password != null).ToList();
        }
    }
}
