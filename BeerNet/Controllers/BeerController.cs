using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BeerNet.Data;
using BeerNet.Managers;
using BeerNet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BeerNet.Controllers
{
    [Route("[controller]/[action]")]
    public class BeerController : Controller
    {
        private readonly UserManager<BeerNetUser> _userManager;
        private readonly IBeerManager _beerManager;

        public BeerController(UserManager<BeerNetUser> userManager, IBeerManager beerManager)
        {
            _userManager = userManager;
            _beerManager = beerManager;
        }

        [HttpGet]
        public IActionResult AllBeers()
        {
            var beers = _beerManager.GetBeers();
            return View("BeerList", beers);
        }

        [HttpGet("{id}")]
        public IActionResult BeerDetails(int id)
        {

            var beer = _beerManager.GetBeer(id);
            if (beer == null)
            {
                return View("AllBeers");
            }
            return View(beer);
        }

        [HttpGet]
        public IActionResult BeerDetails(string beerName)
        {
            var beer = _beerManager.GetBeer(beerName);
            if (beer == null)
            {
                return View("AllBeers");
            }
            return View(beer);
        }

        [Authorize]
        [HttpPost("{id}")]
        public IActionResult AddRating(int id, string beerRateDescription, string ratingRange)
        {
            ratingRange = ratingRange.Replace('.', ',');
            float rate = float.Parse(ratingRange);

            var beer = _beerManager.GetBeer(id);
            if (beer == null)
            {
                return View("BeerList");
            }

            var beerRate = new BeerRate()
            {
                Beer = beer,
                Description = beerRateDescription,
                Rate = rate,
                User = _userManager.GetUserAsync(HttpContext.User).Result
            };

            _beerManager.AddBeerRating(beer, beerRate);

            return RedirectToAction("BeerDetails", new { id = id });
        }

        [Authorize]
        [HttpPost("{beerId}")]
        public IActionResult EditRating(int beerId, string beerRateDescription, string ratingRange)
        {
            ratingRange = ratingRange.Replace('.', ',');
            float rate = float.Parse(ratingRange);

            _beerManager.EditBeerRating(beerId, _userManager.GetUserAsync(User).Result.Email, beerRateDescription, rate);

            return RedirectToAction("BeerDetails", new { id = beerId });
        }
    }
}
