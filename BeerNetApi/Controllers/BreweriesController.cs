using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeerNetApi.Managers;
using BeerNetApi.Models;
using BeerNetApi.Models.PostModels;
using BeerNetApi.SwaggerExamples;
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

        /// <summary>
        /// Returns brewery. List of its beers included
        /// </summary>
        /// <response code="200">Found matching brewery</response>
        /// <response code="204">Wrong breweryId</response>
        [HttpGet]
        [Route("{breweryId}")]
        [ProducesResponseType(typeof(BreweryExample), StatusCodes.Status200OK)]
        public IActionResult Get(int breweryId)
        {
            var brewery = _breweriesManager.GetBrewery(breweryId);
            if (brewery != null)
            {
                return Ok(brewery);
            }
            return NoContent();
        }

        /// <summary>
        /// Adds brewery to db
        /// </summary>
        /// <response code="200">Added brewery successfully</response>
        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public IActionResult Post(BreweryPostModel model)
        {
            Brewery brewery = (Brewery)model;
            _breweriesManager.AddBrewery(brewery);
            return NoContent();
        }

        /// <summary>
        /// Returns list of matching breweries. Beers ignored
        /// </summary>
        /// <response code="200">Found matching breweries</response>
        /// <response code="204">No breweries were found</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<BreweriesExample>), StatusCodes.Status200OK)]
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
