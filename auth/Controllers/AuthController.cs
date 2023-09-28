using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using auth.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using auth.Options;
using Microsoft.Extensions.Options;

namespace auth.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthOptions _options;

        public AuthController(IOptions<AuthOptions> options)
        {
            _options = options.Value;
        }



        [HttpPost("login")]
        public async Task<JsonResult> Login([FromBody] LoginModel model)
        {
            await Task.Delay(500);
            if(model.Login == "test" && model.Password == "test")
            {
                return new JsonResult(new LoginModel.Result { Login = "test", Token = GenerateJwt ("test", "ide")});
            }
            else
            {
                return new JsonResult("No auth");
            }
        }



        [HttpPost("register")]
        public async Task<JsonResult> Register([FromBody] RegisterModel model)
        {
            await Task.Delay(500);
            if(ModelState.IsValid)
            {
                if (model.Login == "test" && model.Password == "test")
                {
                    return new JsonResult("Register.token123...");
                }
                else
                {
                    return new JsonResult(StatusCodes.Status401Unauthorized);
                }
            }
            return new JsonResult("No compare");
        }



        /// <summary>
        /// Токен создатель
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ide"></param>
        /// <returns></returns>
        private string GenerateJwt(string name, string ide)
        {
            var claims = new List<Claim>{ new(ClaimTypes.Name, name) };
            claims.Add(new(ClaimTypes.Sid, ide));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Jwt.SigningKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now + _options.Jwt.Expiration;

            var token = new JwtSecurityToken(
                _options.Jwt.Issuer,
                _options.Jwt.Audience,
                claims,
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
