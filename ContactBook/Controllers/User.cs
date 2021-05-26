using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ContactBook.Domain;
using ContactBook.Domain.Data.Repositories;
using ContactBook.Domain.DTO;
using ContactBook.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ContactBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class User : ControllerBase
    {
        private IConfiguration _config;
        private Cloudinary _cloudinary;
        private IContactRepository _contactRepository;
        private ContactDataContext _db;

        public User(IConfiguration config, IContactRepository contactRepository, ContactDataContext dataContext)
        {
            _config = config;
            Account account = new Account
            {
                Cloud = _config.GetSection("cloudinarySettings:CloudName").Value,
                ApiKey = _config.GetSection("cloudinarySettings:ApiKey").Value,
                ApiSecret = _config.GetSection("cloudinarySettings:ApiSecret").Value
            };

            _cloudinary = new Cloudinary(account);
            _contactRepository = contactRepository;
            _db = dataContext;
        }


        [HttpPatch("photo/id")]
        public async Task<ActionResult> UpdatePhoto(string id, [FromBody] EditPhotoDto contact)
        {
            await _contactRepository.UpdatePhoto(contact);
            return Content("Profile Successfully Updated");
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpPost("photo/{id}")]
        public IActionResult AddUserPhoto(int id, [FromForm] PhotoToAddDto model)
        {
            // if login user id does not match with the id passed

            //if (id != Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            //{
            //    return Unauthorized();
            //}

            var file = model.PhotoFile;
            if (file.Length <= 0)
            {
                return BadRequest("Invalid file size");
            }

            var imageUploadResult = new ImageUploadResult();

            using (var fs = file.OpenReadStream())
            {
                var imageUploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, fs),
                    Transformation = new Transformation().Width(300).Height(300).Crop("fill").Gravity("face")
                };

                imageUploadResult = _cloudinary.Upload(imageUploadParams);
            }

            var publicId = imageUploadResult.PublicId;
            var Url = imageUploadResult.Url.ToString();
            

            return Ok(new { id = publicId, Url });

        }

        [Authorize(AuthenticationSchemes = "Bearer",Roles ="Admin")]
        [HttpGet("all-users")]
        
        public async Task<IEnumerable<ContactDto>> GetContacts([FromQuery] PaginationFilter filter)
        {
            return await _contactRepository.Get(filter);
        }

        [HttpGet("Id/{id}")]
        public async Task<ActionResult<ContactDto>> GetContactById(string id)
        {
            return await _contactRepository.GetById(id);
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<ContactDto>> GetProfileByEmail(string email)
        {
            return await _contactRepository.GetByEmail(email);
        }

        //[Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpPost("add-new")]
        
        public async Task<ActionResult> PostProfile([FromBody] ContactDto contact)
        {
            var newContact = await _contactRepository.Create(contact);
            return CreatedAtAction(nameof(GetContacts), new { id = newContact }, newContact);

        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpPut("update/{id}")]
        public async Task<ActionResult> PutProfile(int id, [FromBody] Contact contact)
        {
            if (id != Convert.ToInt32(contact.Id))
            {
                return BadRequest();
            }

            await _contactRepository.Update(contact);
            return Content("You have successfully updated your contact");
        }


        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpDelete("delete/{id}")]
    
        public async Task<ActionResult> Delete(string id)
        {
            var profileToDelete = await _contactRepository.GetById(id);
            if (profileToDelete == null)
            {
                return NotFound();
            }
            await _contactRepository.Delete(id);
            return Content("You have successfully deleted stuff");

        }


    }
}
