using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Tinder.API.Data;
using Tinder.API.Dtos;
using Tinder.API.Models;

namespace Tinder.API.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper)
        {
            _authRepository = repo;
            _configuration = config;
            _mapper = mapper;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserForRegisterDto user)
        {
            user.Username = user.Username.ToLowerInvariant();
            if (await _authRepository.UserExists(user.Username))
                return BadRequest("Użytkownik już istnieje");
            var userToCreate = _mapper.Map<User>(user);
            var createdUser = await _authRepository.Register(userToCreate, user.Password);

            var userToReturn = _mapper.Map<UserForDetailedDto>(createdUser);
            return CreatedAtRoute("GetUser", new { Id = createdUser.Id }, userToReturn);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserForLoginDto userDto)
        {
            var userFromRepo = await _authRepository.Login(userDto.Username.ToLowerInvariant(), userDto.Password);
            if (userFromRepo == null)
                return Unauthorized();

            //create token
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(12),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            var user = _mapper.Map<UserForListDto>(userFromRepo);

            return Ok(new 
            { 
                token = tokenHandler.WriteToken(token),
                user                
            }); 
        }

    }
}