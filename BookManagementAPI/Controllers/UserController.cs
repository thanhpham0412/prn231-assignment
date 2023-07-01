using Microsoft.AspNetCore.Mvc;
using BusinessObject.Models;
using Repository.Interface;
using Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Formatter;

namespace BookManagementAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IConfiguration config;

        public UserController(IUserRepository _repo, IConfiguration _config)
        {
            userRepository = _repo;
            config = _config;
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] User login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString, role = user.Role });
            }

            return response;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] User register)
        {
            if (userRepository.GetByUsername(register.Username) != null)
            {
                return BadRequest("Username already exists.");
            }

            userRepository.Insert(register);
            userRepository.Save();
            var tokenString = GenerateJSONWebToken(register);
            return Ok(new { token = tokenString });
            
        }

        [HttpGet]
        [EnableQuery(PageSize = 3)]
        [Authorize(Roles = "Admin")]
        public IActionResult Get()
        {
            return Ok(userRepository.GetAll());
        }

        [HttpGet("{id}")]
        [EnableQuery]
        [Authorize(Roles = "Admin, User")]
        public IActionResult GetById([FromODataUri] int id)
        {
            var user = userRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPut]
        [Authorize(Roles = "Admin, User")]
        public IActionResult Update([FromBody] User user)
        {
            var curUser = userRepository.GetById(user.Id);
            if (curUser == null)
            {
                return NotFound();
            }
            curUser.Password = user.Password;

            userRepository.Update(curUser);
            userRepository.Save();
            return Ok();

        }

        private string GenerateJSONWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("username", userInfo.Username),
                new Claim(ClaimTypes.Role, userInfo.Role.ToString())
            };

            var token = new JwtSecurityToken(config["Jwt:Issuer"],
              config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User AuthenticateUser(User login)
        {
            User user = userRepository.GetByUsernameAndPassword(login.Username, login.Password);
            return user;
        }
    }
}
