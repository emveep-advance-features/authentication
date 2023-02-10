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
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("Dd03Majde883bqANauoa983qFNkadaldisahakaFNa983qlsvlaahqlAjskxxianakah");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials
                (
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenItem = tokenHandler.WriteToken(token);
            // var tokenItem = repository.generateToken(model.id);
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