using ASP.Net_JWT_API_Products.Data;
using ASP.Net_JWT_API_Products.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ASP.Net_JWT_API_Products.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly DataBaseContext _context;
        public AuthenticationController(DataBaseContext context)
        {
            _context = context;
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromForm] Login user)
        {
            if (!TryValidateModel(user, nameof(Login)))
                return BadRequest();
            ModelState.ClearValidationState(nameof(Login));
            if (user is null)
            {
                return BadRequest("Invalid user request!!!");
            }
            if (await _context.Logins.AnyAsync(item => item.UserMail == user.UserMail && item.Password == user.Password))
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSetting["JWT:Secret"]));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: ConfigurationManager.AppSetting["JWT:ValidIssuer"],
                    audience: ConfigurationManager.AppSetting["JWT:ValidAudience"],
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(6),
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new JWTTokenResponse { Token = tokenString });
            }
            return Unauthorized();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetValue(int id)
        {
            var value = await _context.Logins.FindAsync(id);
            return Ok(value);
        }

        [HttpPost("Registration")]
        public async Task<ActionResult> Add([FromForm] Login user)
        {
            if (!TryValidateModel(user, nameof(Login)))
                return BadRequest();
            ModelState.ClearValidationState(nameof(Login));
            var addr = new System.Net.Mail.MailAddress(user.UserMail);
            if (addr.Address == user.UserMail)
            {
                if (await _context.Logins.AnyAsync(item => item.UserMail == user.UserMail))
                {
                    return Conflict();
                }
                else
                {
                    await _context.Logins.AddAsync(user);
                    await _context.SaveChangesAsync();
                    string uri = $"/api/registration/{user.Id}";
                    return Created(uri, user);
                }
            }
            else return BadRequest();
        }
    }
}
