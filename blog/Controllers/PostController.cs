﻿using blog.Models;
using blog.Models.Domain;
using blog.Options;
using blog.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace blog.Controllers
{
    [Authorize]
    [Route("api/post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepo _repo;
        private readonly AuthOptions _options;
        private IHttpContextAccessor _context;

        public PostController(IPostRepo repo, IOptions<AuthOptions> options, IHttpContextAccessor context)
        {
            _repo = repo;
            _options = options.Value;
            _context = context;
        }


        /// <summary>
        /// Один пост
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Post> Get(int Id) => await _repo.GetPostById(Id);


        /// <summary>
        /// Все посты пользователя
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<Post>> Get() => await _repo.GetAllPosts(ValidateJwtToken(_context.HttpContext.Request.Headers["Authorization"]
            .FirstOrDefault()?
            .Split(" ")
            .Last()));


        /// <summary>
        /// Создать пост
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostModel model)
        {
            try
            {
                await _repo.AddPostAsync(title: model.title, 
                    description: model.description, 
                    ValidateJwtToken(_context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()));
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            try
            {
                _repo.DeletePost(Id);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        /// <summary>
        /// Валидация токена
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
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
