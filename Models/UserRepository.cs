

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace authentication.Models
{
    public class UserRepository : IUserRepository
    {
        private List<User> _users = new List<User>
        {
            new User 
            {
                id = 1, username = "yahya", password = "yahya1234"            
            },
            new User
            {
                id = 2, username = "irwan", password = "irwan1234"
            }
        };
        public User authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }
            var user = _users.SingleOrDefault(x => x.username == username);
            if (user == null)
            {
                return null;
            }
            return user;
        }

        public string generateToken(int id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes("Dd03Majde883bqANauoa983qFNkadaldisahakaFNa983qlsvlaahqlAjskxxianakah");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials
                (
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public IEnumerable<User> getAll()
        {
            return _users.ToList();
        }

        public User getById(int id)
        {
            return _users.FirstOrDefault(x => x.id == id);
        }
    }
}