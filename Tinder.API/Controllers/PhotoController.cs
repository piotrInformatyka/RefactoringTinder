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
using Microsoft.Extensions.Options;
using Tinder.API.Helper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Tinder.API.Controllers
{
    [Route("users/{userId}/photos")]
    [ApiController]
    [Authorize]

    public class PhotoController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;
        public PhotoController(IUserRepository repository, IMapper mapper, IOptions<CloudinarySettings> cloudOpt)
        {
            _userRepository = repository;
            _mapper = mapper;
            _cloudinaryConfig = cloudOpt;

            Account account = new Account(_cloudinaryConfig.Value.CloudName, 
                _cloudinaryConfig.Value.ApiKey, _cloudinaryConfig.Value.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm]PhotoForCreationDto photoForCreationDto)
        {
            var currentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (userId  != currentId)
            {
                return Unauthorized();
            }
            var userFromRepo = await _userRepository.GetUser(userId);

            var file = photoForCreationDto.File;
            var uploadResult = new ImageUploadResult();
            if(file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500)
                            .Crop("fill").Gravity("face")
                    };
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }
            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDto);
            if(!userFromRepo.Photos.Any(p => p.IsMain))
            {
                photo.IsMain = true;
            }
            userFromRepo.Photos.Add(photo);
            if (await _userRepository.SaveAll())
            {
                var photoForReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtAction(nameof(GetPhoto), new { id = photo.Id}, photoForReturn);
            }
            return BadRequest("Nie można dodać zdjęcia");
        }
        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<ActionResult<PhotoForReturnDto>> GetPhoto([FromRoute]int id)
        {
            var photoFromRepo = await _userRepository.GetPhoto(id);
            var photoForReturn = _mapper.Map<PhotoForReturnDto>(photoFromRepo);
            return Ok(photoForReturn);
        }
        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMain(int userId, int id)
        {
            var currentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (userId != currentId)
            {
                return Unauthorized();
            }
            var user = await _userRepository.GetUser(userId);
            if (!user.Photos.Any(p => p.Id == id))
                return BadRequest();
            var photoFromRepo = await _userRepository.GetPhoto(id);

            if (photoFromRepo.IsMain)
                return BadRequest("To już jest główne zdjęcie");
            var currentMainPhoto = await _userRepository.GetMainPhoto(userId);
            currentMainPhoto.IsMain = false;
            photoFromRepo.IsMain = true;

            if (await _userRepository.SaveAll())
                return NoContent();

            return BadRequest("Nie można ustawić zdjęcia jako głównego");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var user = await _userRepository.GetUser(userId);
            if (!user.Photos.Any(p => p.Id == id))
                return BadRequest();
            var photoFromRepo = await _userRepository.GetPhoto(id);

            if (photoFromRepo.IsMain)
                return BadRequest("Nie można usunąć głównego zdjęcia");
            if (photoFromRepo.public_id != null)
            {
                var deleteParams = new DeletionParams(photoFromRepo.public_id);
                var result = _cloudinary.Destroy(deleteParams);
                if (result.Result == "ok")
                    _userRepository.Delete(photoFromRepo);
            }
            if (photoFromRepo.public_id == null)
                _userRepository.Delete(photoFromRepo);
            if (await _userRepository.SaveAll())
                return Ok();
            return BadRequest("Nie udało się usunąć zdjęcia");
        }
    }
}
