using System;
using System.Linq;
using System.Security.Claims;
using BeerNetApi.Managers;
using BeerNetApi.Models;
using BeerNetApi.Models.PostModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeerNetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public UserController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Get public user data. All user's beer rates included.
        /// </summary>
        /// <response code="200">Found matching user</response>
        /// <response code="400">User not found</response>
        [HttpGet("{login}")]
        [ProducesResponseType(typeof(PublicUserData), StatusCodes.Status200OK)]
        public IActionResult Get(string login)
        {
            var publicUserData = _userManager.GetUserDataByLogin(login);
            if (publicUserData == null)
            {
                return BadRequest($"User with given login ({login}) does not exist");
            }
            return Ok(publicUserData);
        }

        /// <summary>
        /// Updates user's personal informations. Users can update only own data.
        /// </summary>
        /// <response code="200">Data updated successfully</response>
        /// <response code="403">If user tries to update other user's data</response>
        [HttpPut]
        [Authorize]
        public IActionResult Put(PublicUserDataPostModel updatedPublicUserData)
        {
            string loggedUserEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            string loggedUserId = _userManager.GetUserDataByEmail(loggedUserEmail).UserId;
            if (loggedUserId != updatedPublicUserData.UserId)
            {
                // User is logged but tries to update data that belongs to other user
                return StatusCode(403);
            }

            _userManager.UpdateUserData(new PublicUserData(updatedPublicUserData));
            return Ok();
        }
    }
}
