using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BeerNet.Data;
using BeerNet.Managers;
using BeerNet.Models;
using BeerNet.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BeerNet.Controllers
{
    [Route("[controller]/[action]")]
    public class BreweryController : Controller
    {
        private readonly IBreweryManager _breweryManager;

        public BreweryController(IBreweryManager breweryManager)
        {
            _breweryManager = breweryManager;
        }

        [HttpGet]
        public IActionResult AllBreweries()
        {
            var breweries = _breweryManager.GetBreweries();
            return View("BreweryList", breweries);
        }

        public IActionResult BrewerySearch(string breweryName, Country? country)
        {
            var breweries = _breweryManager.AdvancedBrewerySearch(breweryName, country);

            if (breweries.Count == 1)
            {
                return RedirectToAction("BreweryDetails", new { id = breweries[0].Id });
            }
            return View("BreweryList", breweries);
        }

        [HttpGet("{id}")]
        public IActionResult BreweryDetails(int id)
        {
            var brewery = _breweryManager.GetBrewery(id);
            return View(brewery);
        }

        [HttpGet]
        public IActionResult BreweryDetails(string name)
        {
            var brewery = _breweryManager.GetBrewery(name);
            return View(brewery);
        }
    }
}
