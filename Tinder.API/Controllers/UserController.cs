using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Tinder.API.Data;
using Tinder.API.Dtos;
using Tinder.API.Models;
using Microsoft.AspNetCore.Identity;
using Tinder.API.Helper;

namespace Tinder.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("users")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserController(IUserRepository repo, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = repo;
            _mapper = mapper;
           
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userFromRepo = await _userRepository.GetUser(currentUserId);

            userParams.UserId = currentUserId;

            var users =await _userRepository.GetUsers(userParams);
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(usersToReturn);
        }
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<ActionResult<User>> GetUser([FromRoute]int id)
        {
            var user = await _userRepository.GetUser(id);
            var userToReturn = _mapper.Map<UserForDetailedDto>(user);
            return Ok(userToReturn);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody]UserForUpdate userForUpdateDto)
        {
            var currentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (id != currentId)
            {
                return Unauthorized();
            }
            var userFromRepo = await _userRepository.GetUser(id);
            _mapper.Map(userForUpdateDto, userFromRepo);
            if(await _userRepository.SaveAll())
                return NoContent();
            throw new Exception("Aktualizacja się nie powiodła przy zapisywaniu do bazy danych");
        }
        [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int id, int recipientId)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var like = await _userRepository.GetLike(id, recipientId);
            if (like != null)
                return BadRequest("Już lubisz tego użytkownika");
            if (await _userRepository.GetUser(recipientId) == null)
                return NotFound();

            like = new Like
            {
                UserLikesId = id,
                UserIsLikedId = recipientId
            };
            _userRepository.Add<Like>(like);
            if (await _userRepository.SaveAll())
                return Ok();
            return BadRequest("Nie można polubić użytkownika" );
        }
    }
}