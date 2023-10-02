using message.Models;
using message.Models.Domain;
using message.Options;
using message.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace message.Controllers
{
    [Route("api/mess")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly AuthOptions _options;
        private CacheService _cacheService;
        private IHttpContextAccessor _context;

        public MessageController(CacheService cacheService, IOptions<AuthOptions> authoptions, IHttpContextAccessor context)
        {
            _cacheService = cacheService;
            _options = authoptions.Value;
            _context = context;
        }



        [HttpGet]
        public async Task<JsonResult> Get()
        {
            return new JsonResult(await _cacheService.GetMessageList(ValidateJwtToken(_context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last())));
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MessageModel model)
        {
            await _cacheService.SetMessage(new Message {
                Id = Guid.NewGuid().ToString(),
                MessageText = model.Text,
                UserId = ValidateJwtToken(_context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last())
            });
            return Ok();
        }




        public string ValidateJwtToken(string token)
        {
            if (String.IsNullOrEmpty(token))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_options.Jwt.SigningKey);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "id").Value;
                return userId;
            }
            catch
            {
                return null;
            }
        }

    }
}
