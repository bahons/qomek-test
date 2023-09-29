using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using auth.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using auth.Options;
using Microsoft.Extensions.Options;
using auth.DbContext;
using Microsoft.EntityFrameworkCore;
using auth.Models.Domain;

namespace auth.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthOptions _options;
        private readonly AppDbContext _dbContext;

        public AuthController(IOptions<AuthOptions> options, AppDbContext appDbContext)
        {
            _options = options.Value;
            _dbContext = appDbContext;
        }



        [HttpPost("login")]
        public async Task<JsonResult> Login([FromBody] LoginModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    User user = await _dbContext.Users.Where(p => p.Login == model.Login).FirstAsync();
                    if (user == null)
                        return new JsonResult("No Login");
                    else if (user.Password != model.Password)
                        return new JsonResult("No Valid Password");
                    else
                        return new JsonResult(new LoginModel.Result { Login = user.Login, Token = GenerateJwt(user.Login, user.Guid) });
                }
                catch(Exception ex)
                {
                    return new JsonResult( ex.Message );
                }
            }
            return new JsonResult("No Login and Password");
        }



        [HttpPost("register")]
        public async Task<JsonResult> Register([FromBody] RegisterModel model)
        {
            if(ModelState.IsValid)
            {
                User? user = await _dbContext.Users.Where(p => p.Login == model.Login).FirstOrDefaultAsync();
                if (user != null) return new JsonResult("Логин имеется");
                else
                {
                    user = new User { Guid = Guid.NewGuid().ToString(), Login = model.Login, Password = model.Password };
                    await _dbContext.Users.AddAsync(user);
                    await _dbContext.SaveChangesAsync();
                    return new JsonResult(new LoginModel.Result { Login = user.Login, Token = GenerateJwt(user.Login, user.Guid) });
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

            //claims.Add(new(ClaimTypes.Sid, ide));

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
