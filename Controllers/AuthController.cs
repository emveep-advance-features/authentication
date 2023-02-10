using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using authentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace authentication.Controllers
{
    // [Authorize]
    [Route("api/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository repository;

        public AuthController(IUserRepository repository )
        {
            this.repository = repository;
        }
        // [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult authenticate([FromForm] AuthenticateModel model)
        {
            var user = repository.authenticate(model.username, model.password);
            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            
            var tokenItem = repository.generateToken(user.id);
            return Ok(new {
                username = user.username,
                token = tokenItem
            });
        }

        [Authorize]
        [HttpGet("users")]
        public IActionResult getUsers()
        {
            var users = repository.getAll();
            return Ok(users);
        }
    }
}