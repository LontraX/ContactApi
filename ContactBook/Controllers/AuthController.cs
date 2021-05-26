using ContactBook.Domain.Data;
using ContactBook.Domain.DTO;
using ContactBook.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        
        private readonly IConfiguration _config;
        private readonly UserManager<Contact> _usermanager;

        public AuthController(IConfiguration config, UserManager<Contact> userManager)
        {
           
            _config = config;
            _usermanager = userManager;
           
        }


        //[HttpPost]
        //[Route("makeadmin/{id}")]
        //public async Task<IActionResult> MakeAdmin(string id)
        //{
        //    if (await _roleManager.FindByNameAsync("Admin") == null)
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole("Admin"));
        //    }
        //    var user = await _userManager.FindByIdAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound("User does not exist");
        //    }
        //    var res = await _userManager.AddToRoleAsync(user, "Admin");



        //    return Ok("You are now an Admin");



        //}






        [HttpPost("login")]
        public async Task <IActionResult> Login([FromBody] LoginDto model)
        {
            var userGotten = await _usermanager.FindByEmailAsync(model.Email);
            model.Password = "mide";
            model.RememberMe = false;
            var userId = Convert.ToInt32(userGotten.Id);
            var username = userGotten.Email;


            string[] roles = { "User"};
            //model.Email = "mide@gmail.com";
            //model.Password = "Password"; 
            //model.RememberMe = false;

            //var username = "mide";
            //var userId = 1;
            //string[] roles = { "Admin" };
            try
            {

                var token = UtilityClass.GenerateToken(username, userId, model.Email, _config, roles);
                return Ok(token);
            }
            catch (Exception e)
            {
                return BadRequest($"Error:{e.ToString()}");
            }
        }
    }
}
