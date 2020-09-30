using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BeerNet.Data;
using BeerNet.Managers;
using BeerNet.Models;
using BeerNet.Models.Enums;
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
        public IActionResult AllBeers(int currentPageNumber = 1, int rowsPerPage = 5)
        {
            int beersToSkip = rowsPerPage * (currentPageNumber - 1);
            var beers = _beerManager.GetBeers(beersToSkip, rowsPerPage);

            ViewBag.NumberOfPages = (int)Math.Ceiling(_beerManager.GetNumberOfBeers() / (float)rowsPerPage);
            ViewBag.CurrentPageNumber = currentPageNumber;
            return View("BeerList", beers);
        }

        //public IActionResult Search(int currentPage = 1, int rowsPerPage = 2)
        //{
        //    int beersToSkip = rowsPerPage * (currentPage - 1);
        //    var beers = _beerManager.GetBeers(beersToSkip, rowsPerPage);

        //    ViewBag.NumberOfPages = (int)Math.Ceiling(_beerManager.GetNumberOfBeers() / (float)rowsPerPage);
        //    ViewBag.CurrentPage = currentPage;
        //    return View("BeerList", beers);
        //}

        public IActionResult Search(string beerName, string breweryName, Country? country, BeerStyle? beerStyle, float? minExtract, float? maxExtract, float? minAbv, float? maxAbv, int currentPageNumber = 1, int rowsPerPage = 5)
        {
            int beersToSkip = rowsPerPage * (currentPageNumber - 1);
            var beers = _beerManager.AdvancedBeerSearch(beerName, breweryName, country, beerStyle, minExtract, maxExtract, minAbv, maxAbv, beersToSkip, rowsPerPage);

            if (beers.Count == 1)
            {
                return RedirectToAction("BeerDetails", new { id = beers[0].Id });
            }
            ViewBag.NumberOfPages = (int)Math.Ceiling(_beerManager.GetNumberOfBeers(beerName, breweryName, country, beerStyle, minExtract, maxExtract, minAbv, maxAbv) / (float)rowsPerPage);
            ViewBag.CurrentPageNumber = currentPageNumber;

            ViewBag.Parms = new Dictionary<string, string>
            {
                { "beerName", beerName },
                { "breweryName", breweryName },
                { "country", country.ToString() },
                { "beerStyle", beerStyle.ToString() },
                { "minExtract", minExtract.ToString() },
                { "maxExtract", maxExtract.ToString() },
                { "minAbv", minAbv.ToString() },
                { "maxAbv", maxAbv.ToString() },
            };

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
