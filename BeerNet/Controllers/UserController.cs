using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeerNet.Managers;
using BeerNet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BeerNet.Controllers
{
    [Route("[controller]/")]
    public class UserController : Controller
    {
        private readonly IBeerNetUserManager _beerNetUserManager;
        public UserController(IBeerNetUserManager beerNetUserManager)
        {
            _beerNetUserManager = beerNetUserManager;
        }

        [HttpGet("{login}")]
        public IActionResult Index(string login)
        {
            var user = _beerNetUserManager.GetBeerNetUserByLogin(login);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
    }
}
