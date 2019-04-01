using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TimeTrackingServer.Models;
using TimeTrackingServer.Services;

namespace TimeTrackingServer.Controllers
{
    [Authorize]
    [EnableCors("cors")]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        public class AuthenticateRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [AllowAnonymous]
        [EnableCors("cors")]
        [HttpPost(nameof(Authenticate))]
        [Produces("application/json")]
        public async Task<SecurityTokenUser> Authenticate([FromBody]AuthenticateRequest authenticateRequest)
        {
            return await _userService.Authenticate(authenticateRequest.Email, authenticateRequest.Password);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
    }
}
