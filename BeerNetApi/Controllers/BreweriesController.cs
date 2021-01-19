using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeerNetApi.Managers;
using BeerNetApi.Models;
using BeerNetApi.Models.PostModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BeerNetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BreweriesController : ControllerBase
    {
        private readonly IBreweriesManager _breweriesManager;

        public BreweriesController(IBreweriesManager breweriesManager)
        {
            _breweriesManager = breweriesManager;
        }

        /// <returns>
        /// HTTP 200 with json with one brewery if it exists with (list of beers included) <br></br>
        /// or HTTP 204 if beer with given id does not exist
        /// </returns>
        [HttpGet]
        [Route("{breweryId}")]
        public IActionResult Get(int breweryId)
        {
            var brewery = _breweriesManager.GetBrewery(breweryId);
            if (brewery != null)
            {
                return Ok(brewery);
            }
            return NoContent();
        }

        [HttpPost]        
        [Authorize]
        public IActionResult Post(BreweryPostModel model)
        {
            Brewery brewery = (Brewery)model;            
            _breweriesManager.AddBrewery(brewery);
            return NoContent();
        }

        /// <returns>
        /// HTTP 200 with json with specific number of breweries for given filters (beers ignored) <br></br>
        /// or HTTP 204 if no beer was found
        /// </returns>
        [HttpGet]
        public IActionResult Get([FromQuery] BreweryFilter breweryFilter)
        {
            var breweries = _breweriesManager.GetBreweries(breweryFilter);
            if (breweries != null)
            {
                return Ok(breweries);
            }
            return NoContent();
        }
    }
}
