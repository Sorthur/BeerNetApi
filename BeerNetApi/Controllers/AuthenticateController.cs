using BeerNetApi.Models;
using BeerNetApi.SwaggerExamples;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BeerNetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<BeerNetUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticateController(UserManager<BeerNetUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Returns bearer token
        /// </summary>
        /// <response code="200">Logged successfully</response>
        /// <response code="401">No authorization</response>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(AuthorizationExample), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Login);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(model.Login);
            }

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    //new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(30),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        /// <summary>
        /// Registers default user
        /// </summary>
        /// <response code="200">Registered successfully</response>
        /// <response code="409">Registered failed - user already exists</response>
        /// <response code="500">Registration failed</response>
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var usernameExist = await _userManager.FindByNameAsync(model.Login);
            var emailExist = await _userManager.FindByEmailAsync(model.Email);
            if (usernameExist != null || emailExist != null)
            {
                return Conflict(new Response { Status = "Error", Message = "User already exists" });
            }

            var user = new BeerNetUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Login,
                Login = model.Login
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(500, new Response { Status = "Error", Message = $"User registration failed; {result.Errors.First().Description}" });
            }

            CreateMissingRoles();
            await _userManager.AddToRoleAsync(user, UserRoles.User);

            return Ok(new Response { Status = "Success", Message = "User registered successfully" });
        }

        /// <summary>
        /// Registers user with admin role
        /// </summary>
        /// <response code="200">Registered successfully</response>
        /// <response code="409">Registered failed - user already exists</response>
        /// <response code="401">No authorization</response>
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        [Route("registerAdmin")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var usernameExist = await _userManager.FindByNameAsync(model.Login);
            var emailExist = await _userManager.FindByEmailAsync(model.Email);
            if (usernameExist != null || emailExist != null)
            {
                return Conflict(new Response { Status = "Error", Message = "User already exists" });
            }

            var user = new BeerNetUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Login
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(500, new Response { Status = "Error", Message = "User registration failed" });
            }

            CreateMissingRoles();
            await _userManager.AddToRoleAsync(user, UserRoles.Admin);

            return Ok(new Response { Status = "Success", Message = "Admin registered successfully" });
        }

        private async void CreateMissingRoles()
        {
            Type userRoles = typeof(UserRoles);
            foreach (var field in userRoles.GetFields())
            {
                var role = field.GetValue(null).ToString();
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
