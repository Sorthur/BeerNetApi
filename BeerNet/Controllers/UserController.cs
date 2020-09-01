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
        public IActionResult Index(string login, string orderBy, bool sortAscending)
        {
            var user = _beerNetUserManager.GetBeerNetUserByLogin(login);
            if (user == null)
            {
                return NotFound();
            }

            user.BeerRates = SortBeerRates(user.BeerRates, orderBy, sortAscending);

            if (sortAscending == false)
            {
                ViewBag.sortAscending = true;
            }
            else
            {
                ViewBag.sortAscending = false;
            }

            return View(user);
        }

        private List<BeerRate> SortBeerRates(List<BeerRate> beerRates, string orderBy, bool sortAscending)
        {
            switch (orderBy)
            {
                case "Beer":
                    if (sortAscending)
                    {
                        return beerRates.OrderBy(r => r.Beer.Name).ToList();
                    }
                    else
                    {
                        return beerRates.OrderByDescending(r => r.Beer.Name).ToList();
                    }
                case "Style":
                    if (sortAscending)
                    {
                        return beerRates.OrderBy(r => r.Beer.Style).ToList();
                    }
                    else
                    {
                        return beerRates.OrderByDescending(r => r.Beer.Style).ToList();
                    }
                case "Score":
                    if (sortAscending)
                    {
                        return beerRates.OrderBy(r => r.Rate).ToList();
                    }
                    else
                    {
                        return beerRates.OrderByDescending(r => r.Rate).ToList();
                    }
                case "Average":
                    if (sortAscending)
                    {
                        return beerRates.OrderBy(r => r.Beer.AverageRating).ToList();
                    }
                    else
                    {
                        return beerRates.OrderByDescending(r => r.Beer.AverageRating).ToList();
                    }
                default:
                    return beerRates;
            }
        }
    }
}
